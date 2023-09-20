using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.Common;

namespace Unity.Entities.SourceGen.SystemGeneratorCommon
{
    public partial class JobEntityDescription
    {
        public TypeDeclarationSyntax Generate()
        {
            var hasEnableableComponent = true; // HasEnableableComponent();
                                               // Currently, it is never set to false.
                                               // It could be false in cases where the IJobEntity is constructing its own EntityQuery based on the provided parameters,
                                               // in which case it can statically determine at source-generation time whether the query contains any enableable components
                                               // (and thus whether it needs to generate the extra code to handle enabled bits correctly).
                                               // This work has not yet been implemented. (e.g. when enableablebits is turned of with static query generation.)
                                               // Check should be based on whether the query contains enableablebits, not if the parameters does (cause some chunks might still need to check if they are enabled)
                                               // Discussion: https://github.cds.internal.unity3d.com/unity/dots/pull/3217#discussion_r227389
                                               // Also do this for EntitiesSourceFactory.JobStructFor if that is still a thing.

            var inheritsFromBeginEndChunk = m_JobEntityTypeSymbol.InheritsFromInterface("Unity.Entities.IJobEntityChunkBeginEnd");
            var isFullyUnmanaged = m_JobEntityTypeSymbol.IsUnmanagedType && UserExecuteMethodParams.All(p => p.TypeSymbol.IsUnmanagedType);
            string partialStructImplementation =
                $@"[global::System.Runtime.CompilerServices.CompilerGenerated]
                    partial struct {TypeName} : global::Unity.Entities.IJobChunk {", global::Unity.Entities.InternalCompilerInterface.IIsFullyUnmanaged".EmitIfTrue(isFullyUnmanaged)}
                   {{
                        {UserExecuteMethodParams.Select(p => p.TypeHandleFieldDeclaration).SeparateByNewLine()}{Environment.NewLine}
                        {EntityManager()}
                        {ChunkBaseEntityIndices()}

                        [global::System.Runtime.CompilerServices.CompilerGenerated]
                        public void Execute(in ArchetypeChunk chunk, int chunkIndexInQuery, bool useEnabledMask, in Unity.Burst.Intrinsics.v128 chunkEnabledMask)
                        {{
                            {UserExecuteMethodParams.Select(p => p.VariableDeclarationAtStartOfExecuteMethod).SeparateByNewLine()}{Environment.NewLine}
                            {"var shouldExecuteChunk = OnChunkBegin(in chunk, chunkIndexInQuery, useEnabledMask, in chunkEnabledMask);\nif (shouldExecuteChunk){".EmitIfTrue(inheritsFromBeginEndChunk)}
                            int chunkEntityCount = chunk.Count;
                            int matchingEntityCount = 0;
                            {@"if (!useEnabledMask)
                               {".EmitIfTrue(hasEnableableComponent)}
                                    for(int entityIndexInChunk = 0; entityIndexInChunk < chunkEntityCount; ++entityIndexInChunk){Environment.NewLine}
                                    {{
                                        {UserExecuteMethodParams.Where(p => p.RequiresExecuteMethodArgumentSetup).Select(p => p.ExecuteMethodArgumentSetup).SeparateByNewLine()}{Environment.NewLine}
                                        Execute({UserExecuteMethodParams.Select(param => param.ExecuteMethodArgumentValue).SeparateByComma()});
                                        matchingEntityCount++;
                                    }}
                           {$@"}}
                               else
                               {{
                                    int edgeCount = Unity.Mathematics.math.countbits(chunkEnabledMask.ULong0 ^ (chunkEnabledMask.ULong0 << 1)) +
                                                    Unity.Mathematics.math.countbits(chunkEnabledMask.ULong1 ^ (chunkEnabledMask.ULong1 << 1)) - 1;
                                    bool useRanges = edgeCount <= 4;
                                    if (useRanges)
                                    {{
                                        var enabledMask = chunkEnabledMask;
                                        int entityIndexInChunk = 0;
                                        int chunkEndIndex = 0;
                                        while (EnabledBitUtility.GetNextRange(ref enabledMask, ref entityIndexInChunk, ref chunkEndIndex))
                                        {{
                                            while (entityIndexInChunk < chunkEndIndex)
                                            {{
                                                {UserExecuteMethodParams.Where(p => p.RequiresExecuteMethodArgumentSetup).Select(p => p.ExecuteMethodArgumentSetup).SeparateByNewLine()}{Environment.NewLine}
                                                Execute({UserExecuteMethodParams.Select(param => param.ExecuteMethodArgumentValue).SeparateByComma()});
                                                entityIndexInChunk++;
                                                matchingEntityCount++;
                                            }}
                                        }}
                                    }}
                                    else
                                    {{
                                        ulong mask64 = chunkEnabledMask.ULong0;
                                        int count = Unity.Mathematics.math.min(64, chunkEntityCount);
                                        for (int entityIndexInChunk = 0; entityIndexInChunk < count; ++entityIndexInChunk)
                                        {{
                                            if ((mask64 & 1) != 0)
                                            {{
                                                {UserExecuteMethodParams.Where(p => p.RequiresExecuteMethodArgumentSetup).Select(p => p.ExecuteMethodArgumentSetup).SeparateByNewLine()}{Environment.NewLine}
                                                Execute({UserExecuteMethodParams.Select(param => param.ExecuteMethodArgumentValue).SeparateByComma()});
                                                matchingEntityCount++;
                                            }}
                                            mask64 >>= 1;
                                        }}
                                        mask64 = chunkEnabledMask.ULong1;
                                        for (int entityIndexInChunk = 64; entityIndexInChunk < chunkEntityCount; ++entityIndexInChunk)
                                        {{
                                            if ((mask64 & 1) != 0)
                                            {{
                                                {UserExecuteMethodParams.Where(p => p.RequiresExecuteMethodArgumentSetup).Select(p => p.ExecuteMethodArgumentSetup).SeparateByNewLine()}{Environment.NewLine}
                                                Execute({UserExecuteMethodParams.Select(param => param.ExecuteMethodArgumentValue).SeparateByComma()});
                                                matchingEntityCount++;
                                            }}
                                            mask64 >>= 1;
                                        }}
                                    }}
                               }}".EmitIfTrue(hasEnableableComponent)}
                            {"}\nOnChunkEnd(in chunk, chunkIndexInQuery, useEnabledMask, in chunkEnabledMask, shouldExecuteChunk);".EmitIfTrue(inheritsFromBeginEndChunk)}
                        }}

                        {GetScheduleAndRunMethods(isFullyUnmanaged)}
                   }}";

            return (TypeDeclarationSyntax) SyntaxFactory.ParseMemberDeclaration(partialStructImplementation);
        }

        string EntityManager() => "public Unity.Entities.EntityManager __EntityManager;\n".EmitIfTrue(RequiresEntityManagerAccess);

        string ChunkBaseEntityIndices() => "[Unity.Collections.ReadOnly] public Unity.Collections.NativeArray<int> __ChunkBaseEntityIndices;\n".EmitIfTrue(HasEntityIndexInQuery());

        string GetScheduleAndRunMethods(bool createSchedulingCalls)
        {
            var source = @"
                public void Run() => __ThrowCodeGenException();
                public void RunByRef() => __ThrowCodeGenException();
                public void Run(global::Unity.Entities.EntityQuery query) => __ThrowCodeGenException();
                public void RunByRef(global::Unity.Entities.EntityQuery query) => __ThrowCodeGenException();

                Unity.Jobs.JobHandle __ThrowCodeGenException() => throw new global::System.Exception(""This method should have been replaced by source gen."");
            ";
            if (createSchedulingCalls)
            {
                source += @"
                    // Emitted to disambiguate scheduling method invocations
                    public global::Unity.Jobs.JobHandle Schedule(global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
                    public global::Unity.Jobs.JobHandle ScheduleByRef(global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
                    public global::Unity.Jobs.JobHandle Schedule(global::Unity.Entities.EntityQuery query, global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
                    public global::Unity.Jobs.JobHandle ScheduleByRef(global::Unity.Entities.EntityQuery query, global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
                    public void Schedule() => __ThrowCodeGenException();
                    public void ScheduleByRef() => __ThrowCodeGenException();
                    public void Schedule(global::Unity.Entities.EntityQuery query) => __ThrowCodeGenException();
                    public void ScheduleByRef(global::Unity.Entities.EntityQuery query) => __ThrowCodeGenException();

                    public global::Unity.Jobs.JobHandle ScheduleParallel(global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
                    public global::Unity.Jobs.JobHandle ScheduleParallelByRef(global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
                    public global::Unity.Jobs.JobHandle ScheduleParallel(global::Unity.Entities.EntityQuery query, global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
                    public global::Unity.Jobs.JobHandle ScheduleParallelByRef(global::Unity.Entities.EntityQuery query, global::Unity.Jobs.JobHandle dependsOn) => __ThrowCodeGenException();
                    public global::Unity.Jobs.JobHandle ScheduleParallel(global::Unity.Entities.EntityQuery query, global::Unity.Jobs.JobHandle dependsOn, global::Unity.Collections.NativeArray<int> chunkBaseEntityIndices) => __ThrowCodeGenException();
                    public global::Unity.Jobs.JobHandle ScheduleParallelByRef(global::Unity.Entities.EntityQuery query, global::Unity.Jobs.JobHandle dependsOn, global::Unity.Collections.NativeArray<int> chunkBaseEntityIndices) => __ThrowCodeGenException();
                    public void ScheduleParallel() => __ThrowCodeGenException();
                    public void ScheduleParallelByRef() => __ThrowCodeGenException();
                    public void ScheduleParallel(global::Unity.Entities.EntityQuery query) => __ThrowCodeGenException();
                    public void ScheduleParallelByRef(global::Unity.Entities.EntityQuery query) => __ThrowCodeGenException();
                ";
            }

            return source;
        }
    }
}
