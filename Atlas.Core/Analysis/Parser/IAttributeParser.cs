using dnlib.DotNet;

namespace Atlas.Core.Analysis.Parser {
    interface IAttributeParser {
        OptionList ParseAttribute(CustomAttribute attribute);
    }
}