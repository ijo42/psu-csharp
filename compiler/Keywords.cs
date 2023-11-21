using System.Collections.Generic;

namespace Компилятор
{
    class Keywords
    {
        public readonly Dictionary<byte, Dictionary<string, byte>> Kw = new();

        public Keywords()
        {
            var tmp = new Dictionary<string, byte>
            {
                ["do"] = Lexemes.dosy,
                ["if"] = Lexemes.ifsy,
                ["in"] = Lexemes.insy,
                ["of"] = Lexemes.ofsy,
                ["or"] = Lexemes.orsy,
                ["to"] = Lexemes.tosy
            };
            Kw[2] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["end"] = Lexemes.endsy,
                ["var"] = Lexemes.varsy,
                ["div"] = Lexemes.divsy,
                ["and"] = Lexemes.andsy,
                ["not"] = Lexemes.notsy,
                ["for"] = Lexemes.forsy,
                ["mod"] = Lexemes.modsy,
                ["nil"] = Lexemes.nilsy,
                ["set"] = Lexemes.setsy
            };
            Kw[3] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["then"] = Lexemes.thensy,
                ["else"] = Lexemes.elsesy,
                ["case"] = Lexemes.casesy,
                ["file"] = Lexemes.filesy,
                ["goto"] = Lexemes.gotosy,
                ["type"] = Lexemes.typesy,
                ["with"] = Lexemes.withsy
            };
            Kw[4] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["begin"] = Lexemes.beginsy,
                ["while"] = Lexemes.whilesy,
                ["array"] = Lexemes.arraysy,
                ["const"] = Lexemes.constsy,
                ["label"] = Lexemes.labelsy,
                ["until"] = Lexemes.untilsy
            };
            Kw[5] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["downto"] = Lexemes.downtosy,
                ["packed"] = Lexemes.packedsy,
                ["record"] = Lexemes.recordsy,
                ["repeat"] = Lexemes.repeatsy
            };
            Kw[6] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["program"] = Lexemes.programsy
            };
            Kw[7] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["function"] = Lexemes.functionsy
            };
            Kw[8] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["procedure"] = Lexemes.procedurensy
            };
            Kw[9] = tmp;
        }
    }
}