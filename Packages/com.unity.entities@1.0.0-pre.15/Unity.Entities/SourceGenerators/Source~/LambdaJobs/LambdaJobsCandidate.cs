using System;
using System.Collections.Generic;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Unity.Entities.SourceGen.Common;

namespace Unity.Entities.SourceGen.LambdaJobs
{
    public enum LambdaJobKind
    {
        Job,
        Entities
    }

    public static class LambdaJobKindExtensions
    {
        public static string ToName(this LambdaJobKind lambdaJobKind) => lambdaJobKind switch
        {
            LambdaJobKind.Job => "Job.WithCode",
            LambdaJobKind.Entities => "Entities.ForEach",
            _ => throw new ArgumentOutOfRangeException()
        };
    }

    public struct LambdaJobsCandidate : ISystemCandidate
    {
        public LambdaJobKind LambdaJobKind { get; set; }
        public SyntaxNode Node { get; set; }
        public TypeDeclarationSyntax ContainingSystemType { get; set; }
        public Dictionary<string, List<InvocationExpressionSyntax>> MethodInvocations { get; set; }
        public string CandidateTypeName => LambdaJobKind.ToName();
    }
}
