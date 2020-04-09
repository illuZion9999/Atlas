using System.Collections.Generic;
using System.Linq;

namespace Atlas.Core.Analysis
{
    class OptionList : List<Option>
    {
        internal OptionList() { }

        internal OptionList(IEnumerable<Option> list)
        {
            foreach (var item in list)
                Add((Option)item.Clone());
        }

        internal bool Exclude { get; set; }
        internal bool ApplyToMembers { get; set; }
        
        internal void Inherit(OptionList other)
        {
            Exclude = other.Exclude;
            ApplyToMembers = other.ApplyToMembers;
            foreach (var option in other)
            {
                var obj = this.SingleOrDefault(o => o.Name == option.Name);
                if (obj != null)
                {
                    var index = IndexOf(obj);
                    this[index].Apply = option.Apply;
                    for (var i = 0; i < option.Parameters.Count; i++)
                    {
                        var pair = option.Parameters.ElementAt(i);
                        this[index].Parameters[pair.Key] = pair.Value;
                    }
                }
                else Add((Option)option.Clone());
            }
        }
    }
}