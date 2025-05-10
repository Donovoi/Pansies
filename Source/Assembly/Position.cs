namespace PoshCode.Pansies
{
    public class Position : IPsMetadataSerializable
    {
        public bool Absolute { get; set; }

        public int? Line { get; set; }

        public int? Column { get; set; }

        public Position() { }

        public Position(int? line, int? column, bool absolute = false)
        {
            Line = line;
            Column = column;
            Absolute = absolute;
        }

        public Position(string metadata)
        {
            this.FromPsMetadata(metadata);
        }

        public override string ToString()
        {
            if (Line is null && Column is null)
            {
                return string.Empty;
            }


            if (Absolute)
            {
                // For absolute positioning, we can save by doing both
                // But if one is null, use these other absolute positioning commands
                if (Line is null)
                {
                    return "\e" + $"[{Column}G";
                }
                else if (Column is null)
                {
                    return "\e" + $"[{Line}d";
                }
                else
                {
                    return "\e" + $"[{Line};{Column}H";
                }
            }
            else
            {
                if (!(Column is null))
                {
                    if (Column > 0)
                    {
                        return "\e" + $"[{Column}C";
                    }
                    else if (Column < 0)
                    {
                        return "\e" + $"[{-Column}D";
                    }
                }
                if (!(Line is null))
                {
                    if (Line > 0)
                    {
                        return "\e" + $"[{Line}B";
                    }
                    else if (Line < 0)
                    {
                        return "\e" + $"[{-Line}A";
                    }
                }
            }
            return string.Empty;
        }

        public string ToPsMetadata()
        {
            return $"{Line};{Column}{(Absolute ? ";" : string.Empty)}";
        }

        public void FromPsMetadata(string metadata)
        {
            var data = metadata.Split(';');
            if (data.Length == 3)
            {
                Absolute = data[2] == "1";
            }
            if (data.Length >= 2)
            {
                Line = data[0].Length > 0 ? (int?)int.Parse(data[0], System.Globalization.NumberFormatInfo.InvariantInfo) : null;
                Column = data[1].Length > 0 ? (int?)int.Parse(data[1], System.Globalization.NumberFormatInfo.InvariantInfo) : null;
            }
        }
    }
}
