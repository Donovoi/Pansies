using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CodeOwls.PowerShell.Provider.PathNodes;
using PoshCode.Pansies.Palettes;

namespace PoshCode.Pansies.Provider
{
    class EntityProviderRoot : PathNodeBase
    {
        public override IPathValue GetNodeValue()
        {
            return new ContainerPathValue(this, Name);
        }

        public override string Name
        {
            get { return "Entity"; }
        }

        public override IEnumerable<IPathNode> GetNodeChildren(CodeOwls.PowerShell.Provider.PathNodeProcessors.IProviderContext providerContext)
        {

            var drive = providerContext.Drive.Root.Split([':'], 3, StringSplitOptions.RemoveEmptyEntries)[1];
            var name = providerContext.Path.Split([System.IO.Path.DirectorySeparatorChar], 2).LastOrDefault();

            switch (drive) {
                case "NerdFontSymbols":
                    if (string.IsNullOrEmpty(name))
                    {
                        return Entities.NerdFontSymbols.Select(i => new Grapheme(i));
                    }
                    else
                    {
                        return Entities.NerdFontSymbols.Where(i => i.Key == name).Select(i => new Grapheme(i));
                    }

                case "Emoji":
                    if (string.IsNullOrEmpty(name))
                    {
                        return Entities.Emoji.Select(i => new Grapheme(i));
                    }
                    else
                    {
                        return Entities.Emoji.Where(i => i.Key == name).Select(i => new Grapheme(i));
                    }
                case "EscapeSequences":
                    if (string.IsNullOrEmpty(name))
                    {
                        return Entities.EscapeSequences.Select(i => new Grapheme(i));
                    }
                    else
                    {
                        return Entities.EscapeSequences.Where(i => i.Key == name).Select(i => new Grapheme(i));
                    }
                case "Extended":
                default:
                    if (string.IsNullOrEmpty(name))
                    {
                        return Entities.ExtendedCharacters.Select(i => new Grapheme(i));
                    }
                    else
                    {
                        return Entities.ExtendedCharacters.Where(i => i.Key == name).Select(i => new Grapheme(i));
                    }
            }
        }
    }
}
