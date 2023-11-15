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
                ["do"] = LexicalAnalyzer.dosy,
                ["if"] = LexicalAnalyzer.ifsy,
                ["in"] = LexicalAnalyzer.insy,
                ["of"] = LexicalAnalyzer.ofsy,
                ["or"] = LexicalAnalyzer.orsy,
                ["to"] = LexicalAnalyzer.tosy
            };
            Kw[2] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["end"] = LexicalAnalyzer.endsy,
                ["var"] = LexicalAnalyzer.varsy,
                ["div"] = LexicalAnalyzer.divsy,
                ["and"] = LexicalAnalyzer.andsy,
                ["not"] = LexicalAnalyzer.notsy,
                ["for"] = LexicalAnalyzer.forsy,
                ["mod"] = LexicalAnalyzer.modsy,
                ["nil"] = LexicalAnalyzer.nilsy,
                ["set"] = LexicalAnalyzer.setsy
            };
            Kw[3] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["then"] = LexicalAnalyzer.thensy,
                ["else"] = LexicalAnalyzer.elsesy,
                ["case"] = LexicalAnalyzer.casesy,
                ["file"] = LexicalAnalyzer.filesy,
                ["goto"] = LexicalAnalyzer.gotosy,
                ["type"] = LexicalAnalyzer.typesy,
                ["with"] = LexicalAnalyzer.withsy
            };
            Kw[4] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["begin"] = LexicalAnalyzer.beginsy,
                ["while"] = LexicalAnalyzer.whilesy,
                ["array"] = LexicalAnalyzer.arraysy,
                ["const"] = LexicalAnalyzer.constsy,
                ["label"] = LexicalAnalyzer.labelsy,
                ["until"] = LexicalAnalyzer.untilsy
            };
            Kw[5] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["downto"] = LexicalAnalyzer.downtosy,
                ["packed"] = LexicalAnalyzer.packedsy,
                ["record"] = LexicalAnalyzer.recordsy,
                ["repeat"] = LexicalAnalyzer.repeatsy
            };
            Kw[6] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["program"] = LexicalAnalyzer.programsy
            };
            Kw[7] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["function"] = LexicalAnalyzer.functionsy
            };
            Kw[8] = tmp;
            tmp = new Dictionary<string, byte>
            {
                ["procedure"] = LexicalAnalyzer.procedurensy
            };
            Kw[9] = tmp;
        }
    }
}