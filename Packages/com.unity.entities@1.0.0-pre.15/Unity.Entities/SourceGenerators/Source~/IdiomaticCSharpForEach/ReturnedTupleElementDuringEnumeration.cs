namespace Unity.Entities.SourceGen.IdiomaticCSharpForEach
{
    readonly struct ReturnedTupleElementDuringEnumeration
    {
        public readonly string TypeSymbolFullName;
        public readonly string TypeArgumentFullName;
        public readonly string PreferredName;
        public readonly QueryType Type;

        public ReturnedTupleElementDuringEnumeration(
            string typeSymbolFullName,
            string typeArgumentFullName,
            string preferredElementName,
            QueryType type)
        {
            TypeSymbolFullName = typeSymbolFullName;
            TypeArgumentFullName = typeArgumentFullName;
            PreferredName = preferredElementName;
            Type = type;
        }
    }
}
