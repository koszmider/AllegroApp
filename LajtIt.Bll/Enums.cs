using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LajtIt.Bll.Enums
{
    public enum AllegroOptions
    {
        Option1 = 1,//               - aukcja promowana przez pogrubienie tytułu na listingach,
        Option2 = 2,//               - aukcja promowana przez wyróżnienie na listingach,
        Option4 = 4,//               - aukcja promowana przez miniaturkę wyświetlaną na listingach,
        Option8 = 8,//               - aukcja sklepowa,
        Option16 = 16,//                - aukcja usunięta przez administratora serwisu,
        Option32 = 32,//                - wskazanie na fakt, że koszty przesyłki pokrywa kupujący,
        Option64 = 64,//                - wskazanie na fakt, że koszty przesyłki pokrywa sprzedający,
        Option128 = 128,//                 - wybrana forma płatności: zwykły przelew,
        Option256 = 256,//                 - wybrana forma płatności: płatność przy odbiorze,
        Option512 = 512,//                 - maska zdezaktualizowana (zawsze wyłączona),
        Option1024 = 1024,//                  - maska zdezaktualizowana (zawsze wyłączona),
        Option2048 = 2048,//                  - maska zdezaktualizowana (zawsze wyłączona),
        Option4096 = 4096,//                  - maska zdezaktualizowana (zawsze wyłączona),
        Option8192 = 8192,//                  - aukcja promowana na stronie głównej,
        Option16384 = 16384,//                   - sprzedający zgadza się na wysyłkę za granicę,
        Option32768 = 32768,//                   - podano dodatkowe informacje o przesyłce i płatności,
        Option65536 = 65536,//                   - aukcja utworzona z czasem do wystawienia w przyszłości,
        Option131072 = 131072,//                    - aukcja wystawiona za pośrednictwem Allegro WebAPI,
        Option262144 = 262144,//                    - aukcja promowana przez podświetlenie na listingach,
        ItemCategoryPromoted = 524288,//                    - aukcja promowana na stronie kategorii,
        Option1048576 = 1048576,//                     - aukcja prywatna,
        Option2097152 = 2097152,//                     - aukcja wystawiona na otoMoto,
        Option4194304 = 4194304,//                     - wybrana opcja dostawy: przesyłka pocztowa ekonomiczna,
        Option8388608 = 8388608,//                     - wybrana opcja dostawy: przesyłka pocztowa priorytetowa,
        Option16777216 = 16777216,//                      - wybrana opcja dostawy: przesyłka kurierska,
        Option33554432 = 33554432,//                      - wybrana opcja dostawy: odbiór osobisty,
        Option67108864 = 67108864,//                      - maska zdezaktualizowana (zawsze włączona).
    }
}
