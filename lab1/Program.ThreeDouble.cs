namespace ConsoleApp1;

public partial class Program
{
    private class ThreeDouble
    {
        protected double aLen, bLen, cLen;

        public void Round() /* округление полей */
        {
            aLen = (int)aLen;
            bLen = (int)bLen;
            cLen = (int)cLen;
        }

        public ThreeDouble(double aLen, double bLen, double cLen)
        {
            this.aLen = aLen;
            this.bLen = bLen;
            this.cLen = cLen;
        }

        public ThreeDouble(ThreeDouble threeDouble)
        {
            aLen = threeDouble.aLen;
            bLen = threeDouble.bLen;
            cLen = threeDouble.cLen;
        }

        public override string ToString() /* перегрузка метода toString */
        {
            return $"a = {aLen}, b = {bLen}, c = {cLen}";
        }
    }
}