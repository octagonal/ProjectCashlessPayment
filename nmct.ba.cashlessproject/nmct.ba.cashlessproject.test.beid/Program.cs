using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Swelio.Engine;

namespace nmct.ba.cashlessproject.test.beid
{
    class Program
    {
        //Post-build event:
        //copy /Y "$(SolutionDir)dll\*.dll" "$(TargetDir)"

        static void Main(string[] args)
        {
            Card card = null;

            Manager engine = new Manager();
            engine.Active = true;
            CardReader reader = engine.GetReader(0);
            if (reader != null)
            {
                reader.ActivateCard();
                card = reader.GetCard();
                if (card != null)
                {
                    Identity identity = card.ReadIdentity();
                    if (identity != null)
                    {
                        Console.WriteLine("{0}", identity.NationalNumber);
                    }
                }
                reader.DeactivateCard();
            }
            Console.ReadLine();
            card = reader.GetCard();
            if (reader.CardPresent)
                card = reader.GetCard();

            engine.Dispose();
        }
    }
}
