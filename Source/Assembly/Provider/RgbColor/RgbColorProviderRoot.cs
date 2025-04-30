using System;
using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using System.Text;
using CodeOwls.PowerShell.Provider.PathNodes;
using PoshCode.Pansies.Palettes;

namespace PoshCode.Pansies.Provider
{
    class RgbColorProviderRoot : PathNodeBase
    {
        private static NamedPalette <RgbColor> colors = new X11Palette();
        private readonly RgbColorMode mode;

        public RgbColorProviderRoot(RgbColorMode mode)
        {
            this.mode = mode;
        }

        #region unchanged code from previous version
        public override IPathValue GetNodeValue()
        {
            return new ContainerPathValue(this, mode.ToString());
        }

        public override string Name
        {
            get { return mode.ToString(); }
        }

        public override IEnumerable<IPathNode> GetNodeChildren(CodeOwls.PowerShell.Provider.PathNodeProcessors.IProviderContext providerContext)
        {
            var color = providerContext.Path.Split([System.IO.Path.DirectorySeparatorChar], 2).Last();

            if (string.IsNullOrEmpty(color) || color.Contains("*"))
            {

                WildcardPattern wildcard = new WildcardPattern(color, WildcardOptions.IgnoreCase);

                foreach (var name in colors.Names.Where(s => wildcard.IsMatch(s.ToString())))
                {
                    yield return new RgbColorItem(colors.GetValue(name), mode, name);
                }
            }
            else if (StringComparer.OrdinalIgnoreCase.Equals(color, "clear") ||
                    StringComparer.OrdinalIgnoreCase.Equals(color, "default"))
            {
                yield return new RgbColorItem(null, mode, color);
            }
            else
            {
                yield return new RgbColorItem(new RgbColor(color), mode, color);
            }
        }
        #endregion

    }
}
