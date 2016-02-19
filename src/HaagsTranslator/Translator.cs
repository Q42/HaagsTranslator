using System;
using System.Collections.Generic;
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
      new []{"childerswijk", "childâhswijk"},
      new []{"([^o])ei", "$1è"}, // moet voor 'scheveningen' en 'eithoeke', geen 'groeit'
      new []{"koets", "patsâhbak"},
      new []{"Kurhaus", "Koeâhhâhs"}, // moet voor 'au' en 'ou'
      new []{"\\bMaurice\\b", "Mâhpie"}, // moet voor 'au' en 'ou'
      new []{"Hagenezen", "Hageneize"}, // moet na 'ei'
      new []{ "(L|l)unchroom", "$1unsroem" },
      new []{ "\\bThis\\b", "Dis" },
      new []{"\\b(H|h)ighlights\\b", "$1aailaaits"},
      new []{ "\\b(L|l)ast-minute\\b","$1asminnut" },
      new [] {"\\bAirport", "Èâhpogt" },
      new [] { "\\bairport", "èâhpogt" },
      new []{ "(A|a)dvertentie", "$1dvâhtensie" },
      new []{ "event(s|)", "ievent$1" },
      new []{ "Event(s|)", "Ievent$1" },
      new [] { "(F|f)acebook", "$1eisboek" },  
      new [] { "(F|f)avorite", "$1avverietûh" },
      new [] { "(F|f)avoriete", "$1avverietûh" },
      new [] { "(F|f)lagship", "fleksjip" }, 
      new []{ "Jazz", "Djez" },
      new []{ "jazz", "djez" },
      new []{ "(T|t)entoon", "$1etoon" },
      new []{ "(C|c)abaret", "$1abberet" },
      new []{ "(M|m)usical", "$1usikol" },
      new []{"kids", "kindâh" }, // 'kindertips'
      new [] { "(M|m)ovies", "$1oevies" }, 
      new [] { "(P|p)alace", "$1ellus"}, 
      new []{ "(P|p)rivacy", "$1raaivesie" },
      new []{ "policy", "pollesie" },
      new []{ "(S|s)how", "$1jow" },
      new []{ "(S|s)hoppen", "$1joppûh" },
      new [] { "social media", "sausjel miedieja" }, 
      new [] { "(S|s)tores", "$1toâhs" },
      new [] { "(T|t)ouchscreen", "$1atskrien" }, 
      new [] { "(T|t)ouch", "$1ats" }, 
      new [] { "that", "det" },
      new []{ "(V|v)andaag", "$1edaag" },
      new [] { "(V|v)intage", "$1intuts" },
      new [] { "you", "joe" },
      new []{ "(W|w)eekend", "$1iekend" },
      new []{ "(W|w)ork", "$1urrek" },
      new [] {"(B|b)ibliotheek", "$1iebeleteik" },
      new [] { "cadeau", "kado" },
      new []{"\\bfood\\b", "vreite"},
      new []{"fastfood", "snel vreite"},
      new []{"doe je het", "doejenut"},
      new []{"\\bsee\\b", "sie"}, // van 'must see'
      new []{"\\b(M|n)ust\\b", "$1us" }, // van 'must see'
      new []{"(M|m)oeten", "$1otte"}, // moet voor '-en'
      new []{"(w|W)eleens", "$1elles"}, // 'weleens', moet voor 'hagenees'
      new []{"(g|G)ouv", "$1oev"}, // 'gouveneur'
      new []{"heeft", "hep"}, // 'heeft', moet voor 'heef'
      new [] { "(on|i)der([^e])", "$1dâh$2" }, // 'onder', 'Zuiderpark', geen 'bijzondere'
      new [] { "([^a])ndere", "$1ndâhre" }, // 'bijzondere', geen 'andere'
      new []{"ui", "ùi"}, // moet voor 'ooi' zitte n
      new []{ "Ui", "Ùi" },
      new []{"oort", "ogt"}, // 'soort', moet voor '-ort'
      new [] { "([^eo])ert\\b", "$1egt" }, // 'gert'
      new [] { "([^eo])erte", "$1egte" }, // 'concerten'
      new [] { "([^e])(a|o)r(|s)t([^j])" , "$1$2g$3t$4" }, // barst, martin etc., geen 'eerste', 'biertje', 'sport'
      new []{" er aan", " d'ran"}, // 'er aan'
      new []{"(A|a)an het\\b", "$1nnut"}, // 'aan het', moet voor 'gaan'
      new []{ "\\b(A|a)an", "$1n" }, // 'aan', 'aanrennen'
      new []{ "\\b(G|g)aan\\b", "$1an" }, // 'gaan'
      new []{"(H|h)oud\\b", "$1ou"}, // 'houd', moet voor 'oud'
      new []{"(au|ou)w([^e])", "$1$2"}, // 'vrouw', ''flauw', maar zonder 'blauwe'
      new []{"oude", "ouwe"}, // 'goude'
      new []{"\\b(T|t)our\\b", "$1oeâh"},
      new []{"diner\\b", "dinei"},
      new []{"(B|b)oul", "$1oel"}, // 'boulevard'
      new [] {"([^sS])(au|ou)", "$1âh" }, // 'oud', geen 'souvenirs'
      new []{"aci", "assi"}, // 'racist'
      new []{"als een", "assun"}, // 'als een'
      new []{"a(t|l) ik", "a$1$1ik"}, // val ik, at ik
      new []{"alk\\b", "alluk"}, // 'valk'
      new []{"([^a])ars", "$1ags"}, // 'harses', geen 'Haagenaars'
      new []{"oor" ,     "oâh"},
      new []{"aar\\b", "aah" }, // 'waar'
      new []{"(A|a)ar([^io])", "$1ah$2"}, // 'aardappel, 'verjaardag', geen 'waarom', 'waarin'
      new [] { "aar([^i])", "ar$1" }, // wel 'waarom', geen 'waarin'
      new []{"patie", "petie"}, // 'sympatiek'
      new []{"aagd\\b", "aag"}, // 'ondervraagd'
      new []{"(am|at|ig|ig|it|kk|nn)en([^ ,.?!])", "$1e$2"}, // 'en' in een woord, bijv. 'samenstelling', 'eigenlijk', 'buitenstaander', 'statenkwartier', 'dennenweg', 'klokkenluider'

      // woordcombinaties
      new []{"\\b(K|k)an er\\b", "$1andâh"},
      new []{"\\b(K|k)en ik\\b", "$1ennik"},
      new []{"\\b(K|k)en u\\b", "$1ennu"},
      new []{"\\b(K|k)en jij\\b", "$1ejjèh"},
      new []{"\\b(A|a)ls u", "$1ssu"},
      new []{"\\b(M|m)ag het\\b", "$1aggut"},
      new []{"\\bik dacht het\\b", "dachut"},
      new []{"\\b(V|v)an jâh\\b", "$1ajjâh"},
      new []{"\\b(K|k)ijk dan\\b", "$1èktan"},
      new []{"\\b(G|g)aat het\\b", "$1aat-ie"},
      new []{"\\b(M|m)et je\\b", "$1ejje"},
      new []{"\\b(V|v)ind je\\b", "$1ijje"},
      new []{ "\\bmij het\\b", "mènnut"},

      new []{"\\b(A|a)ls er\\b", "$1stâh"},
      new []{"\\b(K|k)(u|a)n j(e|ij) er\\b", "$1ejjedâh"}, // 'ken je er'
      new [] { "\\b(K|k)un je\\b", "$1ajje" }, 

      new []{"ADO Den Haag", "FC De Haag"},
      new []{"ADO", "Adau"},
      new [] {"([^i])atie", "$1asie" }, // 'informatie', geen 'initiatief'
      new []{ "avil", "ave" }, // 'strandpaviljoen'
      new []{"sje\\b", "ssie"}, // 'huisje', moet voor 'asje'
      new []{"\\balleen\\b", "enkelt"},
      new []{"\\bAlleen\\b", "Enkelt"},
      new []{"(A|a)ls je", "$1sje"}, // moet voor 'als'
      new []{"([^v])als\\b",       "$1as"},
      new []{"(b|k|w)ar\\b", "$1âh"},
      new []{ "\\bAls\\b", "As"},
      new []{" bent\\b", " ben"}, // 'ben', geen 'instrument'
      new []{"bote", "baute"}, // 'boterham'
      new [] {"(B|b)roc", "$1rauc" }, // 'brochure'
      new []{"bt\\b", "b"}, // 'hebt'
      new []{"cc", "ks"}, // 'accenten'
      new [] { "chique", "sjieke" },
      new [] {"chure", "sjure" }, // 'brochure'
      new []{"ct", "kt"}, // 'geactualiseerde', 'directie'
      new [] { "Co", "Ko" }, // 'Concerten'
      new [] { "co", "ko" }, // 'concerten', 'collectie'
      new [] { "cu", "ku" }, // 'culturele'
      new [] { "Cu", "Ku" }, // 'culturele'
      new []{"(ch|c|k)t\\b", "$1"}, // woorden eindigend op 'cht', 'ct', 'kt', of met een 's' erachter ('geslachts')
      new []{"(ch|c|k)ts", "$1s"}, // woorden eindigend op 'cht', 'ct', 'kt', of met een 's' erachter ('geslachts')
      new []{"(d|D)at er", "$1attâh"}, // 'dat er'
      new []{"(d|D)at is ", "$1a's "}, // 'dat is'
      new []{"derb", "dâhb"},
      new []{"derd\\b", "dâhd"}, // 'veranderd'
      new []{"(d|D)eze([^l])", "$1eize$2"},
      new []{"dt\\b", "d"}, // 'dt' op het einde van een woord
      new [] {"\\b(B|b)ied\\b", "$1iedt" }, // uitzondering, moet na '-dt'
      new [] { "([^jhm])ds", "$1s" }, // 'scheidsrechter', 'godsdienstige', 'gebedsdienst', geen 'ahdste', 'beroemdste', 'eigentijds'
      new []{"(D|d)y", "$1i"}, // dynamiek
      new []{"eaa", "eiaa"}, // 'ideaal'
      new [] { "ègen", "ège" }, // 'eigentijds', moet voor 'ee'
      new [] { "Eig", "Èg" }, // 'Eigenlijk', moet voor 'ee'
      new [] { "eig", "èg" }, // 'eigenlijk', moet voor 'ee'
      new []{"uee\\b", "uwee"}, // 'prostituee', moet voor '-ee'
      new []{"ueel\\b", "eweil"}, // 'audiovisueel'
      new [] {"uele\\b", "eweile" }, // 'actuele'
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
      new [] { "(D|d)ance", "$1ens" }, // moet na '-ens' 
      new []{"([^ i])eden\\b", "$1eije"}, // geen 'bieden'
      new []{"([^ i])eden", "$1eide" }, // 'hedendaagse'
      new [] { "\\b(E|e)ve", "$1ive" }, // 'evenementen'
      new []{"me(d|t)e", "mei$1e"}, // 'medeklinkers'
      new []{ "eugd", "eug" }, // 'jeugd', 'jeugdprijs'
      new []{"([^o])epot\\b", "$1eipau"}, // 'depot'
      new []{"(e|E)rg\\b", "$1rrag"}, // 'erg', moet voor 'ergens'
      new []{ "([^fnN])(a|o)rm","$1$2rrem" }, // 'platform', 'vormgeving', 'warm', geen 'normale', 'informatie'
      new []{ "(f|N|n)orm", "$1oâhm" }, // 'normale', 'informatie'
      new [] {"(i|I)nter", "$1ntâh" }, // moet voor '-ern'
      new []{"([^evV])er(m|n)", "$1erre$2"}, // kermis', geen 'vermeer', 'vermoeide', 'externe'
      new []{"([^et])(e|E)rg", "$1$2rreg"}, // 'kermis', 'ergens', geen 'achtergelaten', 'neergelegd'
      new [] {"(G|g)eve(r|n)", "$1eive$2" }, // 'Gevers', moet voor '-ers', geen 'gevestigd'
      new []{"([^eo ])ers([^. ,c])", "$1egs$2"}, // 'diverse', 'versie', geen klinkers, geen 'eerste', geen 'verscheen'
      new []{"\\b(V|v)ers\\b", "$1eâhs"}, // 'vers', moet voor -ers}
      new [] { "renstr", "restr" }, // 'herenstraat' (voor koppelwoorden)
      new [] {"([^eio])eder", "$1eider" }, // 'Nederland', geen 'iedereen', 'bloederige'
      new []{"ers\\b", "âhs"}, // 'klinkers'
      new []{"erst", "âhst"}, // 'eerste'
      new [] { "([^eo])eci", "$1eici" }, // 'speciaal'
      new []{ "ese", "eise" }, // 'reserveer'
      new []{ "eiser", "eisâh"}, // 'reserveer'
      new []{ "eur([^e])", "euâh$1"}, // worden eindigend op 'eur', zoals 'deur', 'gouveneurlaan', geen 'kleuren'
      new []{"eurl", "euâhl"}, // worden eindigend op 'eur', zoals 'deur', 'gouveneurlaan'
      new []{"eer", "eâh" }, // 'zweer', 'neer'
      new []{"([^oej])enu", "$1einu"}, // 'menu', geen 'doejenut'
      new []{"elk\\b", "ellek"}, // 'elk'
      new []{"(E|e)xt", "$1kst" }, // 'extra'
      new [] {"\\b(g|G|v|V|h|H)ele\\b", "$1eile" }, // 'vele', 'gele', 'hele'
      new []{"(?=ge)l", "hoi"},
      new []{"([^iok])ele(n|m)", "$1eile$2"}, // 'helemaal', geen 'enkele', 'winkelen'
      new []{"(B|b)eke([^n])", "$1eike$2"}, // 'beker', geen 'bekende'
      new []{"([^ioBbg])eke", "$1eike"}, // geen 'aangekeken' op 'gek', wel 'kek'
      new [] { "([^r])rege", "$1reige" }, // 'gekregen', geen 'berrege'
      new [] { "([^bBIior])e(g|v|p)e(l|n|m| )", "$1ei$2e$3" }, // aangegeven, geen 'geleden', 'uitspreken', 'geknepen', 'goeveneur', 'verdiepen', 'postzegels', 'begeleiding', 'berregen'
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
      new []{"(E|e)x(c|k)", "$1ksk" }, // 'excursies'
      new []{"([^ ])ter([^aern])", "$1tâh$2"}, // 'achtergesteld', geen 'beluisteren', 'literatuur', 'sterren', 'externe'
      new [] {"feli", "feili" }, // 'gefeliciteerd'
      new [] { "(I|i)ndeli", "$1ndeili" }, // 'indeling', geen 'eindelijk', 'wandelingen'
      new []{"(f|p|ie)t\\b", "$1"}, // 'blijft', 'niet', 'betrapt'
      new []{"fd\\b", "f"}, // 'hoofd'
      new [] { "(F|f)eb", "$1eib" }, // 'februari'
      new []{"ngt\\b", "nk"}, // 'hangt'
      new []{"eving", "eiving"}, // 'omgeving'
      new [] { "gje\\b", "ggie"}, // 'dagje'
      new []{"gelegd\\b", "geleige"}, // 'neergelegd'
      new []{"([HhVvr])ee(l|n|t)", "$1ei$2"}, // 'verscheen', 'veel', 'overeenkomsten', 'heet'
      new [] {"(H|h)er([^e])", "$1eâh$2" }, // 'herzien', geen 'herenstraat'
      new []{"(I|i)n het", "$1nnut"}, // 'in het'
      new [] {"ete([^i])", "eite$1" }, // 'hete', 'gegeten', geen 'bibliotheek'
      new []{ "(d|h|l|m|r|t)ea", "$1eija" }, // 'theater'
      new []{"\\bhet\\b", "ut"},
      new []{"Het\\b", "Ut"},
      new [] { "([^eouù])i\\b", "$1ie" }, // 'januari'
      new [] { "ier([^o])", "ieâh$1" }, // 'bierfeest', geen 'hieronymus'
      new [] { "iero([^eo])", "ierau$1" }, // 'hieronymus'
      new []{"ière", "ijerre"}, // 'barriere'
      new []{"ibu", "ibe"}, // 'tribunaal'
      new []{"icke", "ikke" }, // 'tickets'
      new [] { "lijke\\b", "lukke" }, // 'koninklijke'
      new []{"ijgt\\b", "ijg"}, // 'krijgt', moet voor 'ij\\b'
      new [] { "(B|b)ijz", "$1iez" }, // 'bijzondere', moet voor 'bij'
      new []{"ij\\b",  "è"}, // 'zij', 'bij'
      new [] { "([^e])ije(n|)", "$1èje" }, // 'bijenkorf', 'blije', geen 'geleie' 
      new []{"ij([dgkslmnftpvz])",  "è$1"}, // 'knijp', 'vijver', 'stijl', 'vervoersbewijzen'
      new []{"([^euù])ig\\b",    "$1ag"}, // geen 'kreig', 'vliegtuig'
      new [] { "([^euù])igd\\b", "$1ag" }, // gevestigd
      new []{"ilm", "illem" }, // 'film'
      new []{"ilieu", "ejui"}, // 'milieu'
      new []{"ina([^i])", "ine$1" }, // dinamiek, geen 'culinair'
      new []{"inc", "ink"}, // 'incontinentie'
      new []{"io([^oen])", "iau$1"}, // 'audio', geen 'viool', 'station'
      new []{"\\bin m'n\\b", "imme"},
      new [] { "(n|r)atio", "$1asjau" }, // 'internationale'
      new [] { "io([^oen])", "iau$1" }, // 'audio', geen 'viool', 'station', 'internationale'
      new []{ "ir(c|k)", "irrek" }, // 'circus'
      new []{"([^gr])ties\\b", "$1sies"}, // 'tradities', moet voor -isch, geen 'smarties'
      new []{"isch(|e)", "ies$1"},
      new []{"is er", "istâh"},
      new [] { "(p) je\\b", "$1ie" }, // 'loop je'
      new [] { "(g|k) je\\b", "$1$1ie" }, // 'zoek je'
      new [] { "jene", "jenei"}, // 'jenever'
      new []{"jezelf", "je ège"}, // "jezelf"
      new []{"([^(oe)])kje\\b", "$1kkie"}, // 'bakje', moet voor algemeen regel op 'je', TODO, 'bekje'
      new []{"opje\\b", "oppie"}, // 'kopje'
      new []{ "([^ dèijst])je\\b", "$1ie"}, // woorden eindigend op -je', zonder 'asje', 'rijtje', 'avondje', geen 'mejje' 'blèjje'
      new []{"(K|k)an\\b", "$1en"}, // 'kan', geen 'kans', 'kaneel'
      new []{"(K|k)unne", "$1enne"}, // 'kunnen', TODO, wisselen van u / e
      new [] { "(K|k)unt", "$1en" },
      new [] { "orf", "orref" },
      new [] { "oro([^eo])", "orau$1" }, // 'Corona'
      new [] { "Oo([igkm])", "Au$1" }, // 'ook' 
      new []{"oo([difgklmnpst])",         "au$1"}, // 'hoog', 'dood'
      new []{"rij",      "rè"},
      new [] { "tieg", "sieg" }, // 'vakantiegevoel'
      new []{"tie\\b",   "sie"}, // 'directie'
      new []{"enties\\b", "ensies"}, // 'inconsequenties', geen 'romantisch'
      new [] {"(b|B|k|K|m|L|l|M|p|P|t|T|w|W)erk", "$1errek" }, // 'kerk', 'werkdagen', geen 'verkeer'
      new [] { "(f|k)jes\\b", "$1$1ies" }, // 'plekjes'
      new []{"(M|m)'n", "$1e"}, // 'm'n'
      new []{"(M|m)ong", "$1eg"}, // 'mongool'
      new []{"(M|m)ein([^ut])", "$1eint$2"}, // moet na 'ee', geen 'menu', 'gemeentemuseum'
      new []{"mt\\b", "mp"}, // 'komt'
      new [] { "([^o])md", "$1mp" }, // 'beroemdste', geen 'omdat' 
      new []{"lein", "lèn"},
      new []{"\\bliggûh\\b", "leggûh"},
      new []{"\\b(L|l)igge\\b", "$1egge" },
      new []{"\\b(L|l)igt\\b", "$1eg" },
      new []{"lez", "leiz"}, // 'lezer'
      new []{"lf", "lluf"}, // 'zelfde'
      new [] { "ll([ ,.])", "l$1" }, // 'till'
      new [] { "ark(|t)", "arrek" }, // 'park', 'markt', 'marktstraat'
      new []{"ark(|t)\\b", "arrek"}, // 'park', 'markt'
      new []{"\\b(M|m)oet\\b", "$1ot"}, // 'moet', geen 'moeten'
      new []{"nair", "nèâh" }, // 'culinair'
      new []{"neme\\b", "neime" }, // 'nemen', geen 'evenementen''
      new []{"nce", "nse"}, // 'nuance'
      new []{ "\\b(N|n)u\\b", "$1âh"},
      new [] { "ny", "ni" }, // 'hieronymus' 
      new []{"\\bmad", "med"}, // 'madurodam'
      new []{ "oer([^i])", "oeâh$1"}, // 'beroerd', 'hoer', geen 'toerist'
      new []{"(ordt|ord)([^e])", "ogt$2"}, // wordt, word, geen 'worden'
      new []{"orde", "ogde"}, // 'worden'
      new []{"(N|n)(|o)od", "$1aud"}, // 'noodzakelijk'
      new [] { "nirs\\b", "nieâhs" }, // 'souvenirs'
      new []{"l(f|k|m|p)([^a])", "lle$1$2"}, // 'volkslied', 'behulp', geen 'elkaar'
      new []{"olk", "ollek"}, // 'volkslied'
      new []{ "(F|f)olleklore", "$1olklore" },
      new []{ "o(c|k)a", "auka" }, // 'locaties'
      new [] { "([^o])oms", "$1omps" }, // 'aankomsthal'
      new []{ "one(e|i)", "aunei" }, // 'toneel'
      new [] { "oni", "auni" }, // 'telefonische'
      new []{"org\\b", "orrag"}, // 'zorg'
      new []{"orge", "orrage"}, // 'zorgen'
      new []{"\\borg", "oâhg"}, // 'orgineel'}
      new []{"o(v|z)e", "au$1e"},
      new []{"o(b|d|g|k|l|m|p|s|t|v)(i|e)", "au$1$2"}, // 'komen', 'grote', 'over', 'olie', 'notie'
      new []{"O(b|d|g|k|l|m|p|s|t|v)(i|e)", "Au$1$2"}, // zelfde, maar dan met hoofdletter
      new []{ "\\bout", "âht" }, // 'outdoor'
      new []{ "\\bOut", "Âht" }, // 'Outdoor'
      new []{"\\b(V|v)er\\b", "$1eâh"}, // 'ver'
      new []{ "der([^eianrouè])", "dâh$1"},// 'moderne'/'moderrene'
      new [] { "\\b(P|p|T|t)er\\b", "$1eâh" }, // 'per', 'ter'
      new [] { "([^ io])er\\b", "$1âh" }, // 'kanker', geen 'hoer', 'er', 'per', 'hier' , moet voor 'over' na o(v)(e)
      new []{"(P| p)o(^st)" , "$1au$2"}, // 'poltici'
      new []{"p ik\\b", "ppik"}, // 'hep ik'
      new []{"ppen", "ppe" }, // 'poppentheater'
      new [] {"(p|P)ro([^oefks])", "$1rau" }, // 'probleem', geen 'prof', 'prostituee'
      new [] { "(p|P)rofe", "$1raufe" }, // 'professor', 'professioneel'
      new []{"rsch", "sch"}, // 'verschijn'
      new []{"(A|a)rme", "$1rreme"}, // 'arme'
      new []{"re(s|tr)(e|o)", "rei$1$2"}, // 'resoluut', 'retro', 'reserveren'
      new []{"redespa", "reidespe"}, // voor Vredespaleis
      new []{ "(R|r)estâhrant", "$1esterant"},
      new []{"rod", "raud"}, // 'madurodam'
      new [] { "(a|o)rt", "$1gt"},  //'korte' 
      new []{"([Rr])o(?=ma)" , "$1au"}, // voor romantisch, maar haal bijv. rommel eruit
      new []{"inds", "ins"}, // 'sinds'
      new []{"seque", "sekwe"}, // 'inconsequenties'
      new []{"sjes\\b", "ssies"}, // 'huisjes'
      new []{"(S|s)hop", "$1jop" }, // 'shop'
      new []{"stje\\b", "ssie"}, // 'beestje'
      new []{"st(b|d|g|h|j|k|l|m|n|p|v|w|z)", "s$1"}, // 'lastpakken', geen 'str'
      new [] {"(S|s)ouv", "$1oev" }, // 'souvenirs'
      new []{ "\\b(s|S)tr", "$1tg" }, // 'strand', moet 'na st-'
      new []{"([^gr])st\\b", "$1s"}, // 'haast', 'troost', 'gebedsdienst', geen 'barst'
      new []{"tep\\b", "teppie"}, // 'step'
      new []{"té\\b", "tei"}, // 'satè'
      new []{"tion", "sion"}, // 'station'
      new []{"tje\\b", "tsje"}, // 'biertje'
      new []{"to\\b", "tau"}, // moet na 'au'/'ou'
      new []{"(T|t)ram", "$1rem"}, // 'tram'
      new []{"ua", "uwa"}, // 'nuance', 'menstruatie'
      new [] { "(J|j)anu", "$1anne" }, // 'januari', moet na 'ua'
      new []{"ùite\\b", "ùitûh" }, // 'buiten'
      new []{"(u|ù)igt\\b", "$1ig"}, // 'zuigt'
      new []{"(U|u)ren", "$1re"}, // 'uren'
      new []{"ùidâh\\b", "ùiâh"}, // 'klokkenluider'
      new []{ "unch", "uns" }, // 'lunch'
      new []{"urg", "urrag"}, // 'Voorburg'
      new [] { "urs", "ugs" }, // 'excursies'
      new [] { "uur", "uâh" }, // 'literatuurfestival', moet voor '-urf'
      new []{"ur(f|k)", "urre$1"}, // 'Turk','snurkende','surf'
      new [] {"tu([^ât])", "te$1"}, // 'culturele', geen 'tua', 'vintage'
      new []{"\\bvan je\\b", "vajje"},
      new []{"\\bvan (het|ut)\\b", "vannut"},
      new []{"([Vv])er([^aeis])", "$1e$2"}, // wel 'verkoop', geen 'verse', 'veranderd', 'overeenkomsten', 'overige'
      new []{"\\bvaka", "veka"}, // 'vakantie'
      new [] { "vaka", "veka" }, // 'vakantie' 
      new []{"vard\\b", "vâh"}, // 'boulevard'
      new []{"voetbal", "foebal"},
      new []{"we er ", "we d'r "}, // 'we er'
      new []{"\\ber\\b", "d'r"},
      new []{"\\bEr\\b", "D'r"},
      new []{"wil hem\\b", "wil 'm"},
      new []{"(W|w|H|h)ee(t|l)", "$1ei$2"}, // 'heel', 'heet'
      new [] { "yo", "yau" }, // 'yoga' 
      new []{"zal\\b",       "zâh"},
      new []{"z'n" , "ze"}, // 'z'n'
      new []{"\\bzich\\b", "ze ège"}, // 'zich'
      new []{"z(au|o)'n", "zaun"},
      new []{"\\bzegt\\b", "zeg"},
      new []{"zo([^enr])", "zau$1"}, // 'zogenaamd', geen 'zoeken', 'zondag', 'zorgen'
      new []{"'t", "ut"},
      new []{"derr", "dèrr" }, // 'moderne, moet na 'ern'/'ode'
      new []{ "Nie-westegse", "Niet westagse" },
      new []{ "&", "en" },
      new []{"us sie", "us-sie" }, // 'must see'
	  new []{ "\\bThe Hague\\b", "De Heek" }, // moet na 'ee -> ei'
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

#if DEBUG
    /// <summary>
    /// retrieve all regexes and if they hit for the given string
    /// </summary>
    /// <param name="dutch"></param>
    /// <returns></returns>
    public static IEnumerable<Tuple<string, bool>> GetHits(string dutch)
    {
      if (string.IsNullOrEmpty(dutch))
        return Enumerable.Empty<Tuple<string,bool>>();

      var result = new List<Tuple<string, bool>>();
      var haags = dutch;
      foreach (var replacement in TranslationReplacements)
      {
        var original = haags;
        haags = Regex.Replace(original, replacement[0], replacement[1], RegexOptions.CultureInvariant);

        result.Add(Tuple.Create(replacement[0], original.Equals(haags, StringComparison.InvariantCultureIgnoreCase)));
      }
      return result;
    }
#endif
  }
}
