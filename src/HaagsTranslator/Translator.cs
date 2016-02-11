using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace HaagsTranslator
{
  /// <summary>
  /// Translator for translating nl-NL to nl-DH
  /// </summary>
  public static class Translator
  {
    /// <summary>
    /// list of replacement rules, ordered
    /// </summary>
    static readonly string[][] TranslationReplacements = {
      new []{"uitgaan",  "stappen"}, // replaced later voor stappe of stappuh
      new []{"childerswijk", "childâhswijk"},
      new []{"([^o])ei", "$1è"}, // moet voor 'scheveningen' en 'eithoeke', geen 'groeit'
      new []{"Restaurants", "Eithoeke"}, // fixen met hoofdletter
      new []{"cheveningen", "cheiveningâh"},
      new []{"koets", "patsâhbak"},
      new []{"Kurhaus", "Koeâhhâhs"}, // moet voor 'au' en 'ou'
      new []{"Maurice Haak", "Maûpie"}, // moet voor 'au' en 'ou'
      new []{"Hagenezen", "Hageneize"}, // moet na 'ei'
      new []{"\\b(H|h)ighlights\\b", "$1oogtepunten"}, 
      new []{"kids", "kindâh" }, // 'kindertips'
      new []{"doe je het", "doejenut"},
      new []{"\\bsee\\b", "sie"}, // van 'must see'
      new []{"(M|m)oeten", "$1otte"}, // moet voor '-en'
      new []{"(w|W)eleens", "$1elles"}, // 'weleens', moet voor 'hagenees'
      new []{"(g|G)ouv", "$1oev"}, // 'gouveneur'
      new []{"heeft", "hep"}, // 'heeft', moet voor 'heef'
      new []{"(on|i)der", "$1dâh"}, // 'onder', 'Zuiderpark'
      new []{"ui", "ùi"}, // moet voor 'ooi' zitte n
      new []{"oort", "ogt"}, // 'soort', moet voor '-ort'
      new []{"([^e])(e|a|o)r(|s)t([^j])", "$1$2g$3t$4"}, // gert, barst, martin etc., geen 'eerste', 'biertje'
      new []{" er aan", " d'ran"}, // 'er aan'
      new []{"(A|a)an het\\b", "$1nnut"}, // 'aan het', moet voor 'gaan'
      new []{"([^dlt])aan",      "$1an"}, // 'gaan' , geen 'gedaan', 'buitenstaander', 'laan'
      new []{"(H|h)oud\\b", "$1ou"}, // 'houd', moet voor 'oud'
      new []{"(au|ou)w([^e])", "$1$2"}, // 'vrouw', ''flauw', maar zonder 'blauwe'
      new []{"oude", "ouwe"}, // 'goude'
      new []{"\\b(T|t)our\\b", "$1oeâh"},
      new []{"diner\\b", "dinei"},
      new []{"(B|b)oul", "$1oel"}, // 'boulevard'
      new []{"(au|ou)", "âh"}, // 'oud'
      new []{"aci", "assi"}, // 'racist'
      new []{"als een", "assun"}, // 'als een'
      new []{"a(t|l) ik", "a$1$1ik"}, // val ik, at ik
      new []{"alk\\b", "alluk"}, // 'valk'
      new []{"([^a])ars", "$1ags"}, // 'harses', geen 'Haagenaars'
      new []{"oor" ,     "oâh"},
      new []{"aar\\b", "aah" }, // 'waar'
      new []{"(A|a)ar([^io])", "$1ah$2"}, // 'aardappel, 'verjaardag', geen 'waarom', 'waarin'
      new []{"aar", "ar"}, // wel 'waarom'
      new []{"patie", "petie"}, // 'sympatiek'
      new []{"aagd\\b", "aag"}, // 'ondervraagd'
      new []{"(am|at|ig|ig|it|kk|nn)en([^ ,.?!])", "$1e$2"}, // 'en' in een woord, bijv. 'samenstelling', 'eigenlijk', 'buitenstaander', 'statenkwartier', 'dennenweg', 'klokkenluider'

      // woordcombinaties
      new[]{"\\b(K|k)an er\\b", "$1andâh"},
      new[]{"\\b(K|k)en ik\\b", "$1ennik"},
      new[]{"\\b(K|k)en u\\b", "$1ennu"},
      new[]{"\\b(K|k)en jij\\b", "$1ejjèh"},
      new[]{"\\b(A|a)ls u", "$1ssu"},
      new[]{"\\b(M|m)ag het\\b", "$1aggut"},
      new[]{"\\bik dacht het\\b", "dachut"},
      new[]{"\\b(V|v)an jâh\\b", "$1ajjâh"},
      new[]{"\\b(K|k)ijk dan\\b", "$1èktan"},
      new[]{"\\b(G|g)aat het\\b", "$1aat-ie"},
      new[]{"\\b(M|m)et je\\b", "$1ejje"},
      new[]{"\\b(V|v)ind je\\b", "$1ijje"},
      new[]{ "\\bmij het\\b", "mènnut"},

      new[]{"\\b(A|a)ls er\\b", "$1stâh"},
      new[]{"\\b(K|k)(u|a)n j(e|ij) er\\b", "$1ejjedâh"}, // 'ken je er'

      new []{"ADO Den Haag", "FC De Haag"},
      new []{"ADO", "Adau"},
      new []{"sje\\b", "ssie"}, // 'huisje', moet voor 'asje'
      new []{"\\balleen\\b", "enkelt"},
      new []{"\\bAlleen\\b", "Enkelt"},
      new []{"(A|a)ls je", "$1sje"}, // moet voor 'als'
      new []{"([^v])als\\b",       "$1as"},
      new []{"(b|k|w)ar\\b", "$1âh"},
      new []{ "\\bAls\\b", "As"},
      new []{" bent\\b", " ben"}, // 'ben', geen 'instrument'
      new []{"bote", "baute"}, // 'boterham'
      new []{"bt\\b", "b"}, // 'hebt'
      new []{"cc", "ks"}, // 'accenten'
      new []{"ct", "kt"}, // 'geactualiseerde', 'directie'
      new []{"(ch|c|k)t\\b", "$1"}, // woorden eindigend op 'cht', 'ct', 'kt', of met een 's' erachter ('geslachts')
      new []{"(ch|c|k)ts", "$1s"}, // woorden eindigend op 'cht', 'ct', 'kt', of met een 's' erachter ('geslachts')
      new []{"(d|D)at er", "$1attâh"}, // 'dat er'
      new []{"(d|D)at is ", "$1a's "}, // 'dat is'
      new []{"derb", "dâhb"},
      new []{"dere\\b", "dâh"}, // 'andere'
      new []{"derd\\b", "dâhd"}, // 'veranderd'
      new []{"(d|D)eze([^l])", "$1eize$2"}, 
      new []{"dt\\b", "d"}, // 'dt' op het einde van een woord
      new []{"([^h])ds", "$1s"}, // 'scheidsrechter', 'godsdienstige', 'gebedsdienst', geen 'ahdste'
      new []{"(D|d)y", "$1i"}, // dynamiek
      new []{"eaa", "eiaa"}, // 'ideaal'
      new []{"uee\\b", "uwee"}, // 'prostituee', moet voor '-ee'
      new []{"ueel\\b", "eweil"}, // 'audiovisueel'
      new []{"(g|n|l|L|m|M)ee(n|s)" , "$1ei$2"}, // 'geen', 'hagenees', 'lees', 'burgemeester'
      new []{"ee\\b", "ei"}, // met '-ee' op het eind zoals 'daarmee', 'veel'
      new []{"eel", "eil"}, // met '-ee' op het eind zoals 'daarmee', 'veel'
      new []{" is een ", " issun "}, // moet voor ' een '
      new []{"(I|i)n een ", "$1nnun "}, // 'in een', voor ' een '
      new []{"één", "ein"}, // 'één'
      new []{" een ",    " un "},
      new []{"Een ", "Un "},
      new []{" eens", " 'ns"}, // 'eens'
      new []{"([^eo])erd\\b", "$1egd"}, // 'werd', geen 'verkeerd', 'gefeliciteerd', 'beroerd
      new []{"eerd", "eâhd"}, // 'verkeerd'
      new []{"([^k])ee(d|f|g|k|l|m|n|p|s|t)", "$1ei$2"}, // 'bierfeest', 'kreeg', 'greep', geen 'keeper'
      new []{"([^e])ens\\b", "$1es"}, // 'ergens', geen 'weleens'
      new []{"([^ i])eden\\b", "$1eije"}, // geen 'bieden'
      new []{"me(d|t)e", "mei$1e"}, // 'medeklinkers'
      new []{"Eig", "Èg"}, // 'Eigenlijk', TODO hoofdletters
      new []{" eig", "èg"}, // 'eigenlijk'
      new []{"([^o])epot\\b", "$1epau"}, // 'depot'
      new []{"(e|E)rg\\b", "$1rrag"}, // 'erg', moet voor 'ergens'
      new []{"(a|o)rm", "$1rrem"}, // 'platform', 'vormgeving', 'warm'
      new []{"([^evV])er(m|n)", "$1erre$2"}, // kermis', geen 'vermeer', 'vermoeide', 'externe'
      new []{"([^et])(e|E)rg", "$1$2rreg"}, // 'kermis', 'ergens', geen 'achtergelaten', 'neergelegd'
      new []{"([^e ])ers([^. ,c])", "$1egs$2"}, // 'diverse', 'versie', geen klinkers, geen 'eerste', geen 'verscheen'
      new []{"\\b(V|v)ers\\b", "$1eâhs"}, // 'vers', moet voor -ers}
      new []{"ers\\b", "âhs"}, // 'klinkers', 'eerste'
      new []{"erst", "âhst"}, // 'klinkers', 'eerste'
      new []{"eur\\b", "euâh"}, // worden eindigend op 'eur', zoals 'deur', 'gouveneurlaan'
      new []{"eurl", "euâhl"}, // worden eindigend op 'eur', zoals 'deur', 'gouveneurlaan'
      new []{"eer", "eâh" }, // 'zweer', 'neer'
      new []{"([^oej])enu", "$1einu"}, // 'menu', geen 'doejenut'
      new []{"elk\\b", "ellek"}, // 'elk'
      new []{"(?=ge)l", "hoi"},
      new []{"([^io])ele(n|m)", "$1eile$2"}, // 'helemaal', geen 'enkele'
      new []{"(B|b)eke([^n])", "$1eike$2"}, // 'beker', geen 'bekende'
      new []{"([^ioBbg])eke", "$1eike"}, // geen 'aangekeken' op 'gek', wel 'kek'
      new []{"([^io])e(g|v|p)e(l|n|m| )", "$1ei$2e$3"}, // aangegeven, 'gekregen' geen 'geleden', 'uitspreken', 'geknepen', 'goeveneur', 'verdiepen', 'postzegels'
      new []{"alve\\b", "alleve"}, // 'halve', moet na 'aangegeven'
      new []{"\\b(K|k)en\\b", "$1an"}, // moet voor -en
      new []{"([^ ieo])en([.?!])", "$1ûh$2"}, // einde van de zin, haal ' en ', 'doen', 'zien' en 'heen'  eruit
      new []{"([^ bieo])en\\b", "$1e"}, // haal '-en' eruit, geen 'verscheen', 'tien', 'indien', 'ben', 'doen'
      new []{"bben\\b", "bbe"}, // 'hebben'
      new []{"oien\\b", "oie"}, // 'weggooien'
      new []{"([^eio])en(b|h|j|l|m|p|r|v|w|z)", "$1e$2"}, // 'binnenhof', geen 'paviljoenhoeder'
      new []{"([Hh])eb je ", "$1ebbie "}, // voor '-eb'
      new []{"(H|h)eb (un|een)\\b", "$1ep'n"}, // voor '-eb'
      new []{"eb\\b", "ep"},
      new []{"([^ ])ter([^aern])", "$1tâh$2"}, // 'achtergesteld', geen 'beluisteren', 'literatuur', 'sterren', 'externe'
      new []{"(d|f)eli", "$1eili"}, // 'gefeliciteerd', 'indeling'
      new []{"(f|p|ie)t\\b", "$1"}, // 'blijft', 'niet', 'betrapt'
      new []{"fd\\b", "f"}, // 'hoofd'
      new []{"ngt\\b", "nk"}, // 'hangt'
      new []{"eving", "eiving"}, // 'omgeving'
      new []{"gelegd\\b", "geleige"}, // 'neergelegd'
      new []{"([HhVvr])ee(l|n|t)", "$1ei$2"}, // 'verscheen', 'veel', 'overeenkomsten', 'heet'
      new []{"(H|h)er", "$1eâh"}, // 'herzien'
      new []{"(I|i)n het", "$1nnut"}, // 'in het'
      new []{"ete", "eite" }, // 'hete', 'gegeten'
      new []{"\\bhet\\b", "ut"},
      new []{"Het\\b", "Ut"},
      new []{"ier",  "ieâh"}, // 'bierfeest'
      new []{"ière", "ijerre"}, // 'barriere'
      new []{"ibu", "ibe"}, // 'tribunaal'
      new []{"ijgt\\b", "ijg"}, // 'krijgt', moet voor 'ij\\b'
      new []{"ij\\b",  "è"}, // 'zij', 'bij'
      new []{"ij([dgksnfpv])",  "è$1"}, // 'zij', 'knijp', 'vijver'
      new []{"([^euù])ig\\b",    "$1ag"}, // geen 'kreig', 'vliegtuig'
      new []{"ilieu", "ejui"}, // 'milieu'
      new []{"ina", "ine"}, // dinamiek
      new []{"inc", "ink"}, // 'incontinentie'
      new []{"io([^oe])", "iau$1"}, // 'audio', geen 'viool'
      new []{"\\bin m'n\\b", "imme"},
      new []{"([^gr])ties\\b", "$1sies"}, // 'tradities', moet voor -isch, geen 'smarties'
      new []{"isch(|e)", "ies$1"},
      new []{"is er", "istâh"},
      new []{ "(g|k|p) je\\b", "$1ie"}, // 'loop je', 'zoek je'
      new []{"jezelf", "je ège"}, // "jezelf"
      new []{"([^(oe)])kje\\b", "$1kkie"}, // 'bakje', moet voor algemeen regel op 'je', TODO, 'bekje'
      new []{"opje\\b", "oppie"}, // 'kopje'
      new []{"([^ dijst])je\\b", "$1ie"}, // woorden eindigend op -je', zonder 'asje', 'rijtje', 'avondje', geen 'mejje'
      new []{"([^e])ije", "$1èje"}, // 'blije', geen 'geleije'
      new []{"(K|k)an\\b", "$1en"}, // 'kan', geen 'kans', 'kaneel'
      new []{"(K|k)unne", "$1enne"}, // 'kunnen', TODO, wisselen van u / e
      new []{"(K|k)or", "$1og"}, // 'korte'
      new []{"oo([difgklmnpst])",         "au$1"}, // 'hoog', 'dood'
      new []{"rij",      "rè"},
      new []{"tie\\b",   "sie"}, // 'directie'
      new []{"enties\\b", "ensies"}, // 'inconsequenties', geen 'romantisch'
      new []{"erk\\b", "errek"}, // 'kerk'
      new []{"(M|m)'n", "$1e"}, // 'm'n'
      new []{"(M|m)ong", "$1eg"}, // 'mongool'
      new []{"(M|m)ein([^u])", "$1eint$2"}, // moet na 'ee', geen 'menu'
      new []{"mt\\b", "mp"}, // 'komt'
      new []{"lein", "lèn"},
      new []{"\\bliggûh\\b", "leggûh"},
      new []{"\\b(L|l)igge\\b", "$1egge" },
      new []{"\\b(L|l)igt\\b", "$1eg" },
      new []{"lez", "leiz"}, // 'lezer'
      new []{"lf", "lluf"}, // 'zelfde'
      new []{"ark(|t)\\b", "arrek"}, // 'park', 'markt'
      new []{"\\b(M|m)oet\\b", "$1ot"}, // 'moet', geen 'moeten'
      new []{"neme", "neime"}, // 'nemen'
      new []{"nce", "nse"}, // 'nuance'
      new []{"\\bmad", "med"}, // 'madurodam'
      new []{ "oer([^i])", "oeâh$1"}, // 'beroerd', 'hoer', geen 'toerist'
      new []{"(ordt|ord)([^e])", "ogt$2"}, // wordt, word, geen 'worden'
      new []{"orde", "ogde"}, // 'worden'
      new []{"(N|n)(|o)od", "$1aud"}, // 'noodzakelijk'
      new []{"l(f|k|m|p)([^a])", "lle$1$2"}, // 'volkslied', 'behulp', geen 'elkaar'
      new []{"olk", "ollek"}, // 'volkslied'
      new []{"org\\b", "orrag"}, // 'zorg'
      new []{"orge", "orrage"}, // 'zorgen'
      new []{"\\borg", "oâhg"}, // 'orgineel'}
      new []{"ove", "auve"},
      new []{"o(b|d|g|k|l|m|p|s|t|v)(i|e)", "au$1$2"}, // 'komen', 'grote', 'over', 'olie', 'notie'
      new []{"O(b|d|g|k|l|m|p|s|t|v)(i|e)", "Au$1$2"}, // zelfde, maar dan met hoofdletter
      new []{"\\b(V|v)er\\b", "$1eâh"}, // 'ver'
      new []{"der([^eiaou])", "dâh$1"}, 
      new []{"([^ io])er\\b", "$1âh"}, // 'kanker', geen 'hoer', 'er', 'hier' , moet voor 'over' na o(v)(e)
      new []{"(P| p)o(^st)" , "$1au$2"}, // 'poltici'
      new []{"p ik\\b", "ppik"}, // 'hep ik'
      new []{"rsch", "sch"}, // 'verschijn'
      new []{"(A|a)rme", "$1rreme"}, // 'arme'
      new []{"re(s|tr)(e|o)", "rei$1$2"}, // 'resoluut', 'retro', 'reserveren'
      new []{"redespa", "reidespe"}, // voor Vredespaleis
      new []{"rod", "raud"}, // 'madurodam'
      new []{"([Rr])o(?=ma)" , "$1au"}, // voor romantisch, maar haal bijv. rommel eruit
      new []{"inds", "ins"}, // 'sinds'
      new []{"seque", "sekwe"}, // 'inconsequenties'
      new []{"stje\\b", "ssie"}, // 'beestje'
      new []{"st(b|d|g|h|j|k|l|m|n|p|v|w|z)", "s$1"}, // 'lastpakken', geen 'str'
      new []{"([^gr])st\\b", "$1s"}, // 'haast', 'troost', 'gebedsdienst', geen 'barst'
      new []{"tep\\b", "teppie"}, // 'step'
      new []{"té\\b", "tei"}, // 'satè'
      new []{"tje\\b", "tsje"}, // 'biertje'
      new []{"to\\b", "tau"}, // moet na 'au'/'ou'
      new []{"(T|t)ram", "$1rem"}, // 'tram'
      new []{"ua", "uwa"}, // 'nuance', 'menstruatie'
      new []{"(u|ù)igt\\b", "$1ig"}, // 'zuigt'
      new []{"(U|u)ren", "$1re"}, // 'uren'
      new []{"ùidâh\\b", "ùiâh"}, // 'klokkenluider'
      new []{"urg", "urrag"}, // 'Voorburg'
      new []{"ur(f|k)", "urre$1"}, // 'Turk','snurkende','surf'
      new []{"uur", "uâh"},
      new []{"\\bvan je\\b", "vajje"},
      new []{"\\bvan (het|ut)\\b", "vannut"},
      new []{"([Vv])er([^aes])", "$1e$2"}, // wel 'verkoop', geen 'verse', 'veranderd', 'overeenkomsten'
      new []{"\\bvaka", "veka"}, // 'vakantie'
      new []{"vard\\b", "vâh"}, // 'boulevard'
      new []{"voetbal", "foebal"},
      new []{"wart", "wagt"},
      new []{"we er ", "we d'r "}, // 'we er'
      new []{"\\ber\\b", "d'r"}, // moet na andere 'er'
	  new []{"\\bEr\\b", "D'r"}, // moet na andere 'er'
      new []{"wil hem\\b", "wil 'm"},
      new []{"(W|w|H|h)ee(t|l)", "$1ei$2"}, // 'heel', 'heet'
      new []{"zal\\b",       "zâh"},
      new []{"z'n" , "ze"}, // 'z'n'
      new []{"\\bzich\\b", "ze ège"}, // 'zich'
      new []{"z(au|o)'n", "zaun"},
      new []{"\\bzegt\\b", "zeg"},
      new []{"zo([^enr])", "zau$1"}, // 'zogenaamd', geen 'zoeken', 'zondag', 'zorgen'
    };

    /// <summary>
    /// Verify that at least our regexe's fit in the cache of the regex lib
    /// </summary>
    static Translator()
    {
      Regex.CacheSize = Math.Max(Regex.CacheSize, TranslationReplacements.Length);
    }

    /// <summary>
    /// Translate the given string from nl-NL to nl-DH
    /// </summary>
    /// <param name="dutch"></param>
    /// <returns></returns>
    public static string Translate(string dutch)
    {
      if (string.IsNullOrEmpty(dutch))
        return dutch;
      return TranslationReplacements.Aggregate(dutch, (current, replacement) => Regex.Replace(current, replacement[0], replacement[1], RegexOptions.CultureInvariant));
    }
  }
}
