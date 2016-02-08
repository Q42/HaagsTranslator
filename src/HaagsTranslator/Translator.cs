using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HaagsTranslator
{
  public class Translator
  {
    static readonly string[][] translationReplacements = {
    new []{"uitgaan",  "stappen"}, // replaced later voor stappe of stappuh
    new []{"childerswijk", "childâhswijk"},
    new []{"([^o])ei", "$1è"}, // moet voor 'scheveningen' en 'eithoeke', geen 'groeit'
    new []{"Restaurants", "Eithoeke"}, // fixen met hoofdletter
    new []{"cheveningen", "cheiveningâh"},
    new []{"koets", "patsâhbak"},
    new []{"Kurhaus", "Koeâhhâhs"}, // moet voor 'au' en 'ou'
    new []{"Maurice Haak", "Maûpie"}, // moet voor 'au' en 'ou'
    new []{"Hagenezen", "Hageneize"}, // moet na 'ei'
    new []{"doe je het", "doejenut"},
    new []{"(M|m)oeten", "$1otte"}, // moet voor '-en'
    new []{"(w|W)eleens", "$1elles"}, // 'weleens', moet voor 'hagenees'
    new []{"(g|G)ouv", "$1oev"}, // 'gouveneur'
    new []{"heeft", "hep"}, // 'heeft', moet voor 'heef'
    new []{"(on|i)der", "$1dâh"}, // 'onder', 'Zuiderpark'
    new []{"ui", "ùi"}, // moet voor 'ooi' zitte n
    new []{"([^e])(e|a)r(|s)t" , "$1$2g$3t"}, // gert, barst, martin etc., geen 'eerste', ''
    new []{" er aan", " d'ran"}, // 'er aan'
    new []{"(A|a)an het([ ,.])", "$1nnut$2"}, // 'aan het', moet voor 'gaan'
    new []{"([^dlt])aan",      "$1an"}, // 'gaan' , geen 'gedaan', 'buitenstaander', 'laan'
    new []{"(H|h)oud\\b", "$1ou"}, // 'houd', moet voor 'oud'
    new []{"(au|ou)w([^e])", "$1$2"}, // 'vrouw', ''flauw', maar zonder 'blauwe'
    new []{"oude", "ouwe"}, // 'goude'
    new []{"(au|ou)", "âh"}, // 'oud'
    new []{"aci", "assi"}, // 'racist'
    new []{"als een", "assun"}, // 'als een'
    new []{"a(t|l) ik", "a$1$1ik"}, // val ik, at ik
    new []{"alk\\b", "alluk"}, // 'valk'
    new []{"([^a])ars", "$1ags"}, // 'harses', geen 'Haagenaars'
    new []{"oor" ,     "oâh"},
    new []{"(g|n|l|L|m|M)ee(n|s)" , "$1ei$2"}, // 'geen', 'hagenees', 'lees', 'burgemeester'
    new []{"(A|a)ar([^io])", "$1ah$2"}, // 'aardappel, 'verjaardag', geen 'waarom', 'waarin'
    new []{"aar", "ar"}, // wel 'waarom'
    new []{"patie", "petie"}, // 'sympatiek'
    new []{"aagd([ ,.])", "aag$1"}, // 'ondervraagd'
    new []{"(am|at|ig|ig|it|kk|nn)en([^ ,.])", "$1e$2"}, // 'en' in een woord, bijv. 'samenstelling', 'eigenlijk', 'buitenstaander', 'statenkwartier', 'dennenweg', 'klokkenluider'
    new []{"ADO Den Haag", "FC De Haag"},
    new []{"ADO", "Adau"},
    new []{"sje\\b", "ssie"}, // 'huisje', moet voor 'asje'
    new []{"(A|a)ls je", "$1sje"}, // moet voor 'als'
    new []{"(A|a)ls\\b",       "$1s"},
    new []{"(b|k|w)ar\\b", "$1âh"},
    new []{" bent([ ,.])", " ben$1"}, // 'ben', geen 'instrument'
    new []{"bt([ ,.])", "b$1"}, // 'hebt'
    new []{"cc", "ks"}, // 'accenten'
    new []{"ct", "kt"}, // 'geactualiseerde', 'directie'
    new []{"(ch|c|k)t([ ,.s])", "$1$2"}, // woorden eindigend op 'cht', 'ct', 'kt', of met een 's' erachter ('geslachts')
    new []{"(d|D)at er", "$1attâh"}, // 'dat er'
    new []{"(d|D)at is ", "$1a's "}, // 'dat is'
    new []{"dere([ ,.])", "dâh$1"}, // 'andere'
    new []{"derd([ ,.])", "dâhd$1"}, // 'veranderd'
    new []{"dt([ ,.])", "d$1"}, // 'dt' op het einde van een woord
    new []{"ds", "s"}, // 'scheidsrechter', 'godsdienstige', 'gebedsdienst'
    new []{"(D|d)y", "$1i"}, // dynamiek
    new []{"uee([ ,.])", "uwee$1"}, // 'prostituee', moet voor '-ee'
    new []{"ee([ ,.l])", "ei$1"}, // met '-ee' op het eind zoals 'daarmee', 'veel'
    new []{" is een ", " issun "}, // moet voor ' een '
    new []{"(I|i)n een ", "$1nnun "}, // 'in een', voor ' een '
    new []{"één", "ein"}, // 'één'
    new []{" een ",    " un "},
    new []{"Een ", "Un "},
    new []{" eens", " 'ns"}, // 'eens'
    new []{"([^e])erd([ ,.])", "$1egd$2"}, // 'werd', geen 'verkeerd', 'gefeliciteerd'
    new []{"eerd", "eâhd"}, // 'verkeerd'
    new []{"ee(d|f|g|k|l|m|n|s|t)", "ei$1"}, // 'bierfeest', 'kreeg', geen 'keeper'
    new []{"([^e])ens([ ,.])", "$1es$2"}, // 'ergens', geen 'weleens'
    new []{"([^ i])eden([ ,.])", "$1eije$2"}, // geen 'bieden'
    new []{"me(d|t)e", "mei$1e"}, // 'medeklinkers'
    new []{"Eig", "Èg"}, // 'Eigenlijk', todo hoofdletters
    new []{" eig", "èg"}, // 'eigenlijk'
    new []{"(e|E)rg\\b", "$1rrag"}, // 'erg', moet voor 'ergens'
    new []{"([^tv])(e|E)r(m|g)", "$1$2rre$3"}, // 'kermis', 'ergens', geen 'achtergelaten', 'vermoeide'
    new []{"([^e ])ers([^. ,c])", "$1egs$2"}, // 'diverse', 'versie', geen klinkers, geen 'eerste', geen 'verscheen'

    new []{"ers([ ,.t])", "âhs$1"}, // 'klinkers', 'eerste'
    new []{"eur([ ,.l])", "euâh$1"}, // worden eindigend op 'eur', zoals 'deur', 'gouveneurlaan'
    new []{"eer(|d)([ ,.])",   "eâh$1$2"}, // zweer,
    new []{"elk([ ,.])", "ellek$1"}, // 'elk'
    new []{"(?=ge)l", "hoi"},
    new []{"([^io])e(g|v|l|k|p)e(l|n|m| )", "$1ei$2e$3"}, // aangegeven, helemaal, 'gekregen' geen 'geleden', 'uitspreken', 'geknepen', 'goeveneur', 'verdiepen', 'postzegels'
    new []{"alve([ ,.])", "alleve$1"}, // 'halve', moet na 'aangegeven'
    new []{"([^ bieo])en([ ,])",       "$1e$2"}, // haal '-en' eruit, geen 'verscheen', 'tien', 'indien', 'ben', 'doen'
    new []{"bben\\b", "bbe"}, // 'hebben'
    new []{"oien([ ,.])", "oie$1"}, // 'weggooien'
    new []{"([^ ieo])en[.]",       "$1ûh."}, // einde van de zin, haal ' en ', 'doen', 'zien' en 'heen'  eruit
    new []{"([Hh])eb je ", "$1ebbie "}, // voor '-eb'
    new []{"(H|h)eb (un|een)\\b", "$1ep'n"}, // voor '-eb'
    new []{"eb([ ,.])",        "ep$1"},

    new []{"([^ ])ter([^e])", "$1tâh$2"}, // 'achtergesteld', geen 'beluisteren'
    new []{"(d|f)eli", "$1eili"}, // 'gefeliciteerd', 'indeling'
    new []{"(f|p|ie)t([ ,.])", "$1$2"}, // 'blijft', 'niet', 'betrapt'
    new []{"ngt\\b", "nk"}, // 'hangt'
    new []{"([HhVvr])ee(l|n|t)", "$1ei$2"}, // 'verscheen', 'veel', 'overeenkomsten', 'heet'
    new []{"(H|h)er", "$1eâh"}, // 'herzien'
    new []{"(I|i)n het", "$1nnut"}, // 'in het'
    new []{"hete", "heite"}, // 'hete'
    new []{"het([ ,.])", "ut$1"}, // todo, kan dit samen?
    new []{"Het([ ,.])", "Ut$1"},
    new []{"ier",  "ieâh"}, // 'bierfeest'
    new []{"ière", "ijerre"}, // 'barriere'
    new []{"ibu", "ibe"}, // 'tribunaal'
    new []{"ijgt([ ,.])", "ijg$1"}, // 'krijgt', moet voor 'ij([ ])'
    new []{"ij([ dgksnfp])",  "è$1"}, // 'zij', 'knijp'
    new []{"([^e])ig([ ,.])",    "$1ag$2"}, // geen 'kreig'
    new []{"ilieu", "ejui"}, // 'milieu'
    new []{"ina", "ine"}, // dinamiek
    new []{"inc", "ink"}, // 'incontinentie'
    new []{"\\bin m'n\\b", "imme"},
    new []{"isch(|e)", "ies$1"},
    new []{"is er", "istâh"},
    new []{"jezelf", "je ège"}, // "jezelf"
    new []{"([^(oe)])kje\\b", "$1kkie"}, // 'bakje', moet voor algemeen regel op 'je', TODO, 'bekje'
    new []{"([^ dist])je([ .,])", "$1ie$2"}, // woorden eindigend op -je', zonder 'asje', 'rijtje', 'avondje'
    new []{"([^e])ije", "$1èje"}, // 'blije', geen 'geleije'
    new []{"(K|k)an([ ,.])", "$1en$2"}, // 'kan', geen 'kans', 'kaneel'
    new []{"(K|k)unne", "$1enne"}, // 'kunnen', todo, wisselen van u / e
    new []{"(K|k)or", "$1og"}, // 'korte'
    new []{"oo([digklmnpst])",         "au$1"}, // 'hoog', 'dood'
    new []{"rij",      "rè"},
    new []{"tie([ ,.])",   "sie$1"}, // 'directie'
    new []{"enties\\b", "ensies"}, // 'inconsequenties', geen 'romantisch'
    new []{"innenhof", "innehof"},
    new []{"erk\\b", "errek"}, // 'kerk'
    new []{"(M|m)'n", "$1e"}, // 'm'n'
    new []{"(M|m)ong", "$1eg"}, // 'mongool'
    new []{"(M|m)ein", "meint"}, // moet na 'ee'
    new []{"mt\\b", "mp"}, // 'komt'
    new []{"lein", "lèn"},
    new []{"lez", "leiz"}, // 'lezer'
    new []{"lf", "lluf"}, // 'zelfde'
    new []{"ark(|t)([ ,.])", "arrek$2"}, // 'park', 'markt'
    new []{"(M|m)oet([ ,.])", "$1ot$2"}, // 'moet', geen 'moeten'
    new []{"neme", "neime"}, // 'nemen'
    new []{"nce", "nse"}, // 'nuance'
    new []{"oer", "oeâh"}, // 'beroerd', 'hoer'
    new []{"(ordt|ord)([^e])", "ogt$2"}, // wordt, word, geen 'worden'
    new []{"orde", "ogde"}, // 'worden'
    new []{"(N|n)(|o)od", "$1aud"}, // 'noodzakelijk'
    new []{"olk", "ollek"}, // 'volkslied'
    new []{"ove", "auve"},
    new []{"o(b|d|g|k|l|m|p|s|t|v)(i|e)", "au$1$2"}, // 'komen', 'grote', 'over', 'olie', 'notie'
    new []{"O(b|d|g|k|l|m|p|s|t|v)(i|e)", "Au$1$2"}, // zelfde, maar dan met hoofdletter
    new []{"([^ io])er\\b", "$1âh"}, // 'kanker', geen 'hoer', 'er', 'hier' , moet voor 'over' na o(v)(e)
    new []{"(P| p)o(^st)" , "$1au$2"}, // 'poltici'
    new []{"rsch", "sch"}, // 'verschijn'
    new []{"rme", "rreme"}, // 'arme'
    new []{"re(s|tr)o", "rei$1o"}, // 'resoluut', 'retro'
    new []{"redespa", "reidespe"}, // voor Vredespaleis
    new []{"([Rr])o(?=ma)" , "$1au"}, // voor romantisch, maar haal bijv. rommel eruit
    new []{"inds", "ins"}, // 'sinds'
    new []{"seque", "sekwe"}, // 'inconsequenties'
    new []{"stje\\b", "ssie"}, // 'beestje'
    new []{"st(b|d|g|h|j|k|l|m|n|p|v|w|z)", "s$1"}, // 'lastpakken', geen 'str'
    new []{"([^gr])st([ ,.])", "$1s$2"}, // 'haast', 'troost', 'gebedsdienst', geen 'barst'
    new []{"tep\\b", "teppie"}, // 'step'
    new []{"té([ ,.])", "tei$1"}, // 'satè'
    new []{"to([ ,.])", "tau$1"}, // moet na 'au'/'ou'
    new []{"(T|t)ram", "$1rem"}, // 'tram'
    new []{"ua", "uwa"}, // 'nuance', 'menstruatie'
    new []{"(u|ù)igt([ ,.])", "$1ig$2"}, // 'zuigt'
    new []{"(U|u)ren", "$1re"}, // 'uren'
    new []{"ùidâh([ ,.])", "ùiâh$1"}, // 'klokkenluider'
    new []{"urg", "urrag"}, // 'Voorburg'
    new []{"ur(f|k)", "urre$1"}, // 'Turk','snurkende','surf'
    new []{"uur", "uâh"},
    new []{"\\bvan je\\b", "vajje"},
    new []{"\\bvan (het|ut)\\b", "vannut"},
    new []{"([Vv])er([^aes])", "$1e$2"}, // wel 'verkoop', geen 'verse', 'veranderd', 'overeenkomsten'
    new []{"\\bvaka", "veka"}, // 'vakantie'
    new []{"voetbal", "foebal"},
    new []{"wart", "wagt"},
    new []{"we er ", "we d'r "}, // 'we er'
    new []{"wil hem\\b", "wil 'm"},
    new []{"(W|w|H|h)ee(t|l)", "$1ei$2"}, // 'heel', 'heet'
    new []{"zal\\b",       "zâh"},
    new []{"z'n" , "ze"}, // 'z'n'
    new []{"\\bzich\\b", "ze ège"}, // 'zich'
    new []{"z(au|o)'n", "zaun"},
    new []{"zo([^en])", "zau$1"}, // 'zogenaamd', geen 'zoeken', 'zondag'
};

    public Translator()
    {
      Regex.CacheSize = translationReplacements.Length;

    }

    public string Translate(string dutch)
    {
      foreach (var replacement in translationReplacements)
      {
        dutch = Regex.Replace(dutch, replacement[0], replacement[1], RegexOptions.CultureInvariant);
      }
      return dutch;
    }
  }
}
