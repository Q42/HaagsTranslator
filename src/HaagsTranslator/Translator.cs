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
      new []{ "childerswijk", "childâhswijk"},
      new [] { "eider", "èijâh" }, // 'projectleider'
      new []{ "(?<![o])ei", "è"}, // moet voor 'scheveningen' en 'eithoeke', geen 'groeit'
      new []{ "koets", "patsâhbak"},
      new []{ "kopje koffie", "bakkie pleuâh" },
      new []{ "Kopje koffie", "Bakkie pleuâh" },
      new []{ "koffie\\b", "pleuâh" },
      new []{ "Koffie\\b", "Pleuâh" },
      new []{ "Kurhaus", "Koeâhhâhs"}, // moet voor 'au' en 'ou'
      new []{ "\\bMaurice\\b", "Mâhpie"}, // moet voor 'au' en 'ou'
      new []{ "Hagenezen", "Hageneize"}, // moet na 'ei'
      new []{ "(L|l)unchroom", "$1unsroem" },
      new []{ "\\bThis\\b", "Dis" },
      new []{ "\\b(H|h)ighlights\\b", "$1aailaaits"},
      new []{ "\\b(L|l)ast-minute\\b","$1asminnut" },
      new []{"\\bAirport", "Èâhpogt" },
      new []{ "\\bairport", "èâhpogt" },
      new []{ "(A|a)dvertentie", "$1dvâhtensie" },
      new []{ "\\b(B|b)eauty", "$1joetie" },
      new []{ "\\bthe\\b", "de" },
      new []{ "\\b(B|b)east\\b", "$1ies" },
      new []{ "(B|b)each", "$1ietsj"},
      new []{ "Bites", "Bèts" },
      new []{ "Cuisine", "Kwiesien"},
      new []{ "cuisine", "kwiesien"},
      new []{ "Europese", "Euraipeise"},
      new []{ "(?<![z])event(s|)", "ievent$1" }, // geen 'zeventig'
      new []{ "Event(s|)", "Ievent$1" },
      new []{ "(F|f)acebook", "$1eisboek" },
      new []{ "(F|f)avorite", "$1avverietûh" },
      new []{ "(F|f)avoriete", "$1avverietûh" },
      new []{ "(F|f)lagship", "fleksjip" },
      new []{ "Jazz", "Djes" },
      new []{ "jazz", "djes" },
      new []{ "(T|t)entoon", "$1etoon" },
      new []{ "(C|c)abaret", "$1abberet" },
      new []{ "(M|m)usical", "$1usikol" },
      new []{ "kids", "kindâh" }, // 'kindertips'
      new []{ "(M|m)ovies", "$1oevies" },
      new []{ "(O|o)rigin", "$1rresjin" }, // 'originele
      new []{ "(P|p)alace", "$1ellus"},
      new []{ "(P|p)rivacy", "$1raaivesie" },
      new []{ "policy", "pollesie" },
      new []{ "\\b(R|r)oots\\b", "$1oets" },
      new []{ "SEA LIFE", "SIELÈF" },
      new []{ "(S|s)how", "$1jow" },
      new []{ "(S|s)hoppen", "$1joppûh" },
      new []{ "(S|s)kiën", "$1kieje"},
      new []{ "(S|s)tores", "$1toâhs" },
      new []{ "(T|t)ouchscreen", "$1atskrien" },
      new []{ "(T|t)ouch", "$1ats" },
      new []{ "that", "det" },
      new []{ "(T|t)ripadvisor", "$1ripetfaaisoâh" },
      new []{ "(V|v)andaag", "$1edaag" },
      new []{ "\\b(V|v)erder\\b", "$1eâhdahs"},
      new []{ "(V|v)intage", "$1intuts" },
      new []{ "you", "joe" },
      new []{ "(W|w)eekend", "$1iekend" },
      new []{ "(W|w)ork", "$1urrek" },
      new []{ "(B|b)ibliotheek", "$1iebeleteik" },
      new []{ "(F|f)ood", "$1oet"},
      new []{ "mondkapje", "bekbedekkâh"},
      new []{ "Mondkapje", "Bekbedekkâh"},
      new []{ "doe je het", "doejenut"},
      new []{ "\\bsee\\b", "sie"}, // van 'must see'
      new []{ "\\b(M|n)ust\\b", "$1us" }, // van 'must see'
      new []{ "(M|m)oeten", "$1otte"}, // moet voor '-en'
      new []{ "(w|W)eleens", "$1elles"}, // 'weleens', moet voor 'hagenees'
      new []{ "(g|G)ouv", "$1oev"}, // 'gouveneur'
      new []{ "heeft", "hep"}, // 'heeft', moet voor 'heef'
      new []{ "(on|i)der(?!e)", "$1dâh" }, // 'onder', 'Zuiderpark', geen 'bijzondere'
      new []{ "(?<![ao])ndere", "ndâhre" }, // 'bijzondere', geen 'andere'
      new []{ "uier\\b", "uiâh" }, // 'sluier', moet voor 'ui'
      new []{ "ui", "ùi"}, // moet voor 'ooi' zitte n
      new []{ "Ui", "Ùi" },
      new []{ "oort", "ogt"}, // 'soort', moet voor '-ort'
      new []{ "(?<![eo])ert\\b", "egt" }, // 'gert'
      new []{ "\\b(V|v)ert", "$1et"}, // 'vertegenwoordiger', moet voor '-ert'
      new []{ "(?<![eo])erte", "egte" }, // 'concerten'
      new []{ "(?<![eo])(a|o)r(|s)t(?!j)", "$1g$2t" }, // barst, martin etc., geen 'eerste', 'biertje', 'sport', 'voorstellingen'
      new []{ " er aan", " d'ran"}, // 'er aan'
      new []{ "(A|a)an het\\b", "$1nnut"}, // 'aan het', moet voor 'gaan'
      new []{ "\\b(A|a)an", "$1n" }, // 'aan', 'aanrennen'
      new []{ "\\b(G|g)aan\\b", "$1an" }, // 'gaan'
      new []{ "(H|h)oud\\b", "$1ou"}, // 'houd', moet voor 'oud'
      new []{ "(au|ou)w(?!e)", "$1"}, // 'vrouw', ''flauw', maar zonder 'blauwe'
      new []{ "oude", "ouwe"}, // 'goude'
      new []{ "\\b(T|t)our\\b", "$1oeâh"},
      new []{ "diner\\b", "dinei"},
      new []{ "(B|b|R|r)ou(l|t)", "$1oe$2"}, // 'boulevard','routes'
      new []{ "o(e|u)r\\b", "oeâh"}, // 'broer', 'retour', moet voor 'au|ou'
      new []{ "oer(?![aieou])", "oeâh"}, // 'beroerd', 'hoer', geen 'toerist', 'stoere, moet voor 'au|ou'
      new []{ "(?<![e])(au|ou)(?![v])", "âh" }, // 'oud', geen 'souvenirs', 'cadeau', 'bureau', 'routes'
      new []{ "Ou", "Âh" }, // 'Oud'
      new []{ "aci", "assi"}, // 'racist'
      new []{ "als een", "assun"}, // 'als een'
      new []{ "a(t|l) ik", "a$1$1ik"}, // val ik, at ik
      new []{ "alk\\b", "alluk"}, // 'valk'
      new []{ "(?<![a])ars", "ags"}, // 'harses', geen 'Haagenaars'
      new []{ "oor" ,     "oâh"},
      new []{ "(A|a)ar(?![io])", "$1ah"}, // 'waar, 'aardappel, 'verjaardag', geen 'waarom', 'waarin'
      new []{ "aar(?![i])", "ar" }, // wel 'waarom', geen 'waarin'
      new []{ "patie", "petie"}, // 'sympatiek'
      new []{ "aagd\\b", "aag"}, // 'ondervraagd'
      new []{ "(am|at|ig|ig|it|kk|nn)en(?![ ,.?!])", "$1e"}, // 'en' in een woord, bijv. 'samenstelling', 'eigenlijk', 'buitenstaander', 'statenkwartier', 'dennenweg', 'klokkenluider'

      // woordcombinaties
      new []{ "\\b(K|k)an er\\b", "$1andâh"},
      new []{ "\\b(K|k)en ik\\b", "$1ennik"},
      new []{ "\\b(K|k)en u\\b", "$1ennu"},
      new []{ "\\b(K|k)en jij\\b", "$1ejjèh"},
      new []{ "\\b(A|a)ls u", "$1ssu"},
      new []{ "\\b(M|m)ag het\\b", "$1aggut"},
      new []{ "\\bik dacht het\\b", "dachut"},
      new []{ "\\b(V|v)an jâh\\b", "$1ajjâh"},
      new []{ "\\b(K|k)ijk dan\\b", "$1èktan"},
      new []{ "\\b(G|g)aat het\\b", "$1aat-ie"},
      new []{ "\\b(M|m)et je\\b", "$1ejje"},
      new []{ "\\b(V|v)ind je\\b", "$1ijje"},
      new []{ "\\bmij het\\b", "mènnut"},
      new []{ "\\b(A|a)ls er\\b", "$1stâh"},
      new []{ "\\b(K|k)(u|a)n j(e|ij) er\\b", "$1ejjedâh"}, // 'ken je er'
      new []{ "\\b(K|k)un je\\b", "$1ajje" },
      new []{"\\bje ([^ ]+) je", "je $1 je ège" },
      new []{ "ADO Den Haag", "FC De Haag"},
      new []{ "ADO", "Adau"},
      new []{ "(?<![i])atie(?![fkv])", "asie" }, // 'informatie', geen 'initiatief', 'kwalitatieve', 'automatiek'
      new []{ "avil", "ave" }, // 'strandpaviljoen'
      new []{ "sje\\b", "ssie"}, // 'huisje', moet voor 'asje'
      new []{ "\\balleen\\b", "enkelt"},
      new []{ "\\bAlleen\\b", "Enkelt"},
      new []{ "(A|a)ls je", "$1sje"}, // moet voor 'als'
      new []{ "athe", "atte"}, // 'kathedraal'
      new []{ "(?<![v])als\\b", "as"},
      new []{ "(b|k|w)ar\\b", "$1âh"},
      new []{ "\\bAls\\b", "As"},
      new []{ " bent\\b", " ben"}, // 'ben', geen 'instrument'
      new []{ "bote", "baute"}, // 'boterham'
      new []{"(B|b)roc", "$1rauc" }, // 'brochure'
      new []{ "bt\\b", "b"}, // 'hebt'
      new []{ "cc", "ks"}, // 'accenten'
      new []{ "chique", "sjieke" },
      new []{"chure", "sjure" }, // 'brochure'
      new []{ "c(a|o|t|r)", "k$1"}, // 'geactualiseerde', 'directie', 'crisis'
      new []{ "C(a|o|t|r)", "K$1" }, // 'Concerten', 'Cadeau'
      new []{ "(c|k)or\\b", "$1oâh" }, // 'decor'
      new []{ "(?<![.])c(a|o)", "k$1" }, // 'concerten', 'cadeau', 'collectie', geen '.com'
      new []{ "\\bkoro", "kerau"}, // 'corona'
      new []{ "cu", "ku" }, // 'culturele'
      new []{ "Cu", "Ku" }, // 'culturele'
      new []{ "(ch|c|k)t\\b", "$1"}, // woorden eindigend op 'cht', 'ct', 'kt', of met een 's' erachter ('geslachts')
      new []{ "(ch|c|k)t(?![aeiouâ])", "$1"}, // woorden eindigend op 'cht', 'ct', 'kt', of met een 's' erachter ('geslachts')
      new []{ "(?<![Gg])(e|r)gt\\b", "$1g"}, // 'zorgt', 'legt'
      new []{ "(d|D)at er", "$1attâh"}, // 'dat er'
      new []{ "(d|D)at is ", "$1a's "}, // 'dat is'
      new []{ "denst", "dest" },
      new []{ "derb", "dâhb"},
      new []{ "derd\\b", "dâhd"}, // 'veranderd'
      new []{ "(?i)(d)eze(?![l])", "$1eize"},
      new []{ "dt\\b", "d"}, // 'dt' op het einde van een woord
      new []{"\\b(B|b)ied\\b", "$1iedt" }, // uitzondering, moet na '-dt'
      new []{ "(D|d)y", "$1i"}, // dynamiek
      new []{ "eaa", "eiaa"}, // 'ideaal'
      new []{ "eau\\b", "o" }, // 'cadeau', 'bureau'
      new []{ "ègent", "èget" }, // 'eigentijds', moet voor 'ee', geen 'dreigend'
      new []{ "Eig", "Èg" }, // 'Eigenlijk', moet voor 'ee'
      new []{ "eig", "èg" }, // 'eigenlijk', moet voor 'ee'
      new []{ "uee\\b", "uwee"}, // 'prostituee', moet voor '-ee'
      new []{ "ueel\\b", "eweil"}, // 'audiovisueel'
      new []{ "uele\\b", "eweile" }, // 'actuele'
      new []{ "(g|n|l|L|m|M)ee(n|s)" , "$1ei$2"}, // 'geen', 'hagenees', 'lees', 'burgemeester'
      new []{ "ee\\b", "ei"}, // met '-ee' op het eind zoals 'daarmee', 'veel'
      new []{ "eel", "eil"}, // met '-ee' op het eind zoals 'daarmee', 'veel'
      new []{ " is een ", " issun "}, // moet voor ' een '
      new []{ "(I|i)n een ", "$1nnun "}, // 'in een', voor ' een '
      new []{ "één", "ein"}, // 'één'
      new []{ " een ",    " un "},
      new []{ "Een ", "Un "},
      new []{ " eens", " 'ns"}, // 'eens'
      new []{ "(?<![eo])erd\\b", "egd"}, // 'werd', geen 'verkeerd', 'gefeliciteerd', 'beroerd
      new []{ "eerd", "eâhd"}, // 'verkeerd'
      new []{ "(?<![k])ee(d|f|g|k|l|m|n|p|s|t)", "ei$1"}, // 'bierfeest', 'kreeg', 'greep', geen 'keeper'
      new []{ "ands\\b", "ens"}, // 'hands', moet voor 'ds'
      new []{ "(?<![èijhm])ds(?![ceèt])", "s" }, // moet na 'ee','godsdienstige', 'gebedsdienst', geen 'ahdste', 'boodschappen', 'beroemdste', 'eigentijds', 'weidsheid', 'reeds', 'strandseizoen', 'wedstrijd'
      new []{ "(?<![eh])ens\\b", "es"}, // 'ergens', geen 'weleens', 'hands/hens'
      new []{ "(D|d)ance", "$1ens" }, // moet na '-ens' 
      new []{ "(?<![ hi])eden\\b", "eije"}, // geen 'bieden'. 'bezienswaardigheden'
      new []{ "(?<![ bgi])eden", "eide" }, // 'hedendaagse', geen 'bedenken'
      new []{ "\\b(E|e)ve", "$1ive" }, // 'evenementen'
      new []{ "(?<![a])(g|m|M|R|r)e(d|t|n)e(?![e])", "$1ei$2e"}, // 'medeklinkers', 'generatie', 'rede', geen 'Hagenees', 'meneer'
      new []{ "eugd", "eug" }, // 'jeugd', 'jeugdprijs'
      new []{ "(?<![o])epot\\b", "eipau"}, // 'depot'
      new []{ "(e|E)rg\\b", "$1rrag"}, // 'erg', moet voor 'ergens'
      new []{ "(?<![fnN])(a|o)rm","$1rrem" }, // 'platform', 'vormgeving', 'warm', geen 'normale', 'informatie'
      new []{ "(f|N|n)orm", "$1oâhm" }, // 'normale', 'informatie'
      new []{ "(i|I)nter", "$1ntâh" }, // moet voor '-ern'
      new []{ "elden", "elde"}, // 'zeeheldenkwartier'
      new []{ "oeter", "oetâh" }, // 'Zoetermeer', moet voor 'kermis'
      new []{ "erm\\b", "errum"}, // 'scherm', moet voor 'kermis'
      new []{ "(?<![eoptvV])er(m|n)", "erre$1"}, // kermis', geen 'vermeer', 'vermoeide', 'toernooi', 'externe', 'supermarkt', termijn
      new []{ "(?<![etv])(e|E)rg(?!ez)", "$1rreg"}, // 'kermis', 'ergens', geen 'achtergelaten', 'neergelegd', 'overgebleven', 'ubergezellige'
      new []{ "ber(?![eoiuaâè])", "bâh"}, // 'ubergezellige', moet na '-erg', geen 'beraden'
      new []{ "(?<![e])(t|V|v)ers(?![clt])", "$1egs"}, // 'vers', 'versie', 'diverse', geen 'gevers', 'verscheen', 'eerste', 
      new []{ "(G|g)eve(r|n)", "$1eive$2" }, // 'Gevers', moet na 'vers', geen 'gevestigd'
      new []{ "renstr", "restr" }, // 'herenstraat' (voor koppelwoorden)
      new []{ "(?<![eIio])eder", "eider" }, // 'Nederland', geen 'iedereen', 'bloederige', 'Iedere'
      new []{ "(?<![eio])ers\\b", "âhs"}, // 'klinkers'
      new []{ "(?<![v])ers(c|t)", "âhs$1"}, // 'eerste', 'bezoekerscentrum', geen 'verschaffen'
      new []{ "erwt", "erret" }, // 'erwtensoep'
      new []{ "(?<![eo])eci", "eici" }, // 'speciaal'
      new []{ "ese", "eise" }, // 'reserveer'
      new []{ "eiser", "eisâh"}, // 'reserveer'
      new []{ "eur\\b", "euâh"}, // worden eindigend op 'eur', zoals 'deur', 'gouveneurlaan', geen 'kleuren'
      new []{ "eur(?![eio])", "euâh"}, // worden eindigend op 'eur', zoals 'deur', 'gouveneurlaan', geen 'kleuren', 'goedkeuring', 'euro
      new []{ "eur(i|o)", "euâhr$1"}, // 'goedkeuring', 'euro'
      new []{ "eurl", "euâhl"}, // worden eindigend op 'eur', zoals 'deur', 'gouveneurlaan'
      new []{ "eer", "eâh" }, // 'zweer', 'neer'
      new []{ "elk\\b", "ellek"}, // 'elk'
      new []{ "(E|e)xt", "$1kst" }, // 'extra'
      new []{ "(H|h)ele", "$1eile"}, // 'gehele', 'hele'
      new []{ "\\b(g|G|v|V)ele\\b", "$1eile" }, // 'vele', 'gele', 'hele'
      new []{ "ebut", "eibut" }, // 'debuteren', geen 'debuut'
      new []{ "nele", "neile" }, // 'originele'
      new []{ "\\b(D|d)elen", "$1eile"}, // 'delen', geen 'wandelen'
      new []{ "sdelen", "sdeile"}, // 'geslachtsdelen', geen 'wandelen'
      new []{ "(?<![diokrs])ele(n|m)", "eile$1"}, // 'helemaal', geen 'enkele', 'winkelen', 'wandelen', 'borrelen', 'beginselen'
      new []{ "(B|b)eke(?![n])", "$1eike"}, // 'beker', geen 'bekende'
      new []{ "(?<![ioBbg])eke", "eike"}, // geen 'aangekeken' op 'gek', wel 'kek'
      new []{ "(?<![r])rege", "reige" }, // 'gekregen', geen 'berrege'
      new []{ "(?<![bBIior])e(g|v|p)e(l|n|m| )", "ei$1e$2" }, // aangegeven, geen 'geleden', 'uitspreken', 'geknepen', 'goeveneur', 'verdiepen', 'postzegels', 'begeleiding', 'berregen'
      new []{ "dige", "dege"}, // 'vertegenwoordiger', moet na 'ege'
      new []{ "alve\\b", "alleve"}, // 'halve', moet na 'aangegeven'
      new []{ "\\b(K|k)en\\b", "$1an"}, // moet voor -en
      new []{ "(a|o)ien\\b", "$1ie"}, // 'uitwaaien', geen 'zien'
      new []{ "(?<![ ieo])en([.?!])", "ûh$1"}, // einde van de zin, haal ' en ', 'doen', 'zien' en 'heen'  eruit
      new []{ "(?<![ bieoh])en\\b", "e"}, // haal '-en' eruit, geen 'verscheen', 'tien', 'indien', 'ben', 'doen', 'hen'
      new []{ "bben\\b", "bbe"}, // 'hebben'
      new []{ "oien\\b", "oie"}, // 'weggooien'
      new []{ "enso", "eso" }, // 'erwtensoep'
      new []{ "(?<![eio])enm(?![e])", "em" }, // 'kinderboekenmuseum', geen 'kenmerken'
      new []{ "(?<![eiovV])en(b|h|j|l|p|r|v|w|z)", "e$1"}, // 'binnenhof', geen 'paviljoenhoeder', 'venlo'
      new []{ "([Hh])eb je ", "$1ebbie "}, // voor '-eb'
      new []{ "(H|h)eb (un|een)\\b", "$1ep'n"}, // voor '-eb'
      new []{ "(?<![eu])eb\\b", "ep"},
      new []{ "(E|e)x(c|k)", "$1ksk" }, // 'excursies'
      new []{ "(?<![ s])teri", "tâhri" }, // 'karakteristieke'
      new []{ "(?<![ sS])ter(?![aeirn])", "tâh"}, // 'achtergesteld', geen 'beluisteren', 'literatuur', 'sterren', 'externe', 'sterker', 'karakteristieke'
      new []{ "feli", "feili" }, // 'gefeliciteerd'
      new []{ "(I|i)ndeli", "$1ndeili" }, // 'indeling', geen 'eindelijk', 'wandelingen'
      new []{ "(f|p)t\\b", "$1"}, // 'blijft', 'betrapt'
      new []{ "\\b(N|n)iet\\b", "$1ie" }, // 'niet', geen 'geniet'
      new []{ "fd\\b", "f"}, // 'hoofd'
      new []{ "(F|f)eb", "$1eib" }, // 'februari'
      new []{ "ngt\\b", "nk"}, // 'hangt'
      new []{ "eving", "eiving"}, // 'omgeving'
      new []{ "gje\\b", "ggie"}, // 'dagje'
      new []{ "go(r)", "gau$1" }, // 'algoritme'
      new []{ "gelegd\\b", "geleige"}, // 'neergelegd'
      new []{ "([HhVvr])ee(l|n|t)", "$1ei$2"}, // 'verscheen', 'veel', 'overeenkomsten', 'heet'
      new []{ "(H|h)er(?![er])", "$1eâh" }, // 'herzien', geen 'herenstraat', 'scherm'
      new []{ "(I|i)n het", "$1nnut"}, // 'in het'
      new []{ "\\b(E|e)te", "$1ite"}, // 'eten'
      new []{ "(?<![ior])ete(?![i])", "eite" }, // 'hete', 'gegeten', geen 'bibliotheek','erretensoep', 'koffietentjes', 'genieten, 'roetes (routes)'
      new []{ "(d|h|l|m|r|t)ea(?![m])", "$1eija" }, // 'theater', geen 'team'
      new []{ "\\bhet\\b", "ut"},
      new []{ "Het\\b", "Ut"},
      new []{ "(?<![eouù])i\\b", "ie" }, // 'januari'
      new []{ "ieri", "ieâhra"}, // 'plezierig'
      new []{ "(?<![uù])ier(?!(a|e|i|ony))", "ieâh" }, // 'bierfeest', 'hieronder', geen 'hieronymus', 'plezierig', 'dieren', 'sluier'
      new []{ "iero(?!e|o|nd)", "ierau" }, // 'hieronymus', geen 'hieronder'
      new []{ "ière", "ijerre"}, // 'barriere'
      new []{ "ibu", "ibe"}, // 'tribunaal'
      new []{ "icke", "ikke" }, // 'tickets'
      new []{ "ijgt\\b", "ijg"}, // 'krijgt', moet voor 'ij\\b'
      new []{ "(B|b)ijz", "$1iez" }, // 'bijzondere', moet voor 'bij'
      new []{ "ij\\b",  "è"}, // 'zij', 'bij'
      new []{ "(?<![e])ije(n|)", "èje" }, // 'bijenkorf', 'blije', geen 'geleie' 
      new []{ "(B|b)ij", "$1è" }, // 'bijbehorende'
      new []{ "\\blijk\\b", "lèk" }, // 'lijk' , geen 'eindelijk' ('-lijk')
      new []{ "(D|d|K|k|R|r|W|w|Z|z)ijk", "$1èk"}, // 'wijk', geen '-lijk'
      new []{ "ij([dgslmnftpvz])",  "è$1"}, // 'knijp', 'vijver', 'stijl', 'vervoersbewijzen', geen '-lijk'
      new []{ "(?<![euù])ig\\b", "ag"}, // geen 'kreig', 'vliegtuig'
      new []{ "tigdù", "tagdù"}, // 'vijftigduizend'
      new []{ "lige\\b", "lage"}, // 'gezellige'
      new []{ "(?<![euù])igd\\b", "ag" }, // gevestigd
      new []{ "ilm", "illem" }, // 'film'
      new []{ "ilieu", "ejui"}, // 'milieu'
      new []{ "inc(k|)", "ink"}, // 'incontinentie', 'binckhorst'
      new []{ "io(?![oen])", "iau"}, // 'audio', geen 'viool', 'station'
      new []{ "\\bin m'n\\b", "imme"},
      new []{ "(n|r)atio", "$1asjau" }, // 'internationale'
      new []{ "io\\b", "iau" }, // 'audio', geen 'viool', 'station', 'internationale'
      new []{ "io(?![oen])", "iau" }, // 'audio', geen 'viool', 'station', 'internationale'
      new []{ "ir(c|k)", "irrek" }, // 'circus'
      new []{ "(?<![gr])ties\\b", "sies"}, // 'tradities', moet voor -isch, geen 'smarties'
      new []{ "isch(|e)", "ies$1"},
      new []{ "is er", "istâh"},
      new []{ "ap je\\b", "appie" }, // 'stap je'
      new []{ "(p) je\\b", "$1ie" }, // 'loop je', geen 'stap je'
      new []{ "(g|k) je\\b", "$1$1ie" }, // 'zoek je'
      new []{ "jene", "jenei"}, // 'jenever'
      new []{ "jezelf", "je ège"}, // "jezelf"
      new []{ "(?<![oe])kje\\b", "kkie"}, // 'bakje', moet voor algemeen regel op 'je', TODO, 'bekje'
      new []{ "olg", "olleg"}, // 'volgens'
      new []{ "(i|o)(k|p)je\\b", "$1$2$2ie"}, // 'kopje', 'gokje', 'tipje' 
      new []{ "(?<![ deèijst])je\\b", "ie"}, // woorden eindigend op -je', zonder 'asje', 'rijtje', 'avondje', geen 'mejje' 'blèjje', 'skiën'
      new []{ "(K|k)an\\b", "$1en"}, // 'kan', geen 'kans', 'kaneel'
      new []{ "(K|k)unne", "$1enne"}, // 'kunnen', TODO, wisselen van u / e
      new []{ "(K|k)unt", "$1en" },
      new []{ "(K|k)led", "$1leid"}, // 'kleding'
      new []{ "orf", "orref" },
      new []{ "oro(?![eo])", "orau" }, // 'Corona'
      new []{ "Oo([igkm])", "Au$1" }, // 'ook' 
      new []{ "oo([difgklmnpst])", "au$1"}, // 'hoog', 'dood'
      new []{ "lo\\b", "lau"}, // 'venlo'
      new []{ "([RrNn])ij", "$1è"}, // 'Nijhuis'
      new []{ "tieg", "sieg" }, // 'vakantiegevoel'
      new []{ "(?<![e])tie\\b",   "sie"}, // 'directie', geen 'beauty'
      new []{ "enties\\b", "ensies"}, // 'inconsequenties', geen 'romantisch'
      new []{ "erpe", "errepe"}, // 'modeontwerper'
      new []{ "(b|B|k|K|m|L|l|M|p|P|t|T|w|W)erk", "$1errek" }, // 'kerk', 'werkdagen', geen 'verkeer'
      new []{ "(f|k)jes\\b", "$1$1ies" }, // 'plekjes'
      new []{ "(M|m)'n", "$1e"}, // 'm'n'
      new []{ "(M|m)ong", "$1eg"}, // 'mongool'
      new []{ "(M|m)ein(?![ut])", "$1eint"}, // moet na 'ee', geen 'menu', 'gemeentemuseum'
      new []{ "mt\\b", "mp"}, // 'komt'
      new []{ "(?<![oO])md(?![e])", "mp" }, // 'beroemdste', geen 'omdat', 'beroemde
      new []{ "lair(?![e])", "lèh"}, // geen 'spectaculaire'
      new []{ "ulaire", "elère"}, // 'spectaculaire'
      new []{ "lein", "lèn"},
      new []{ "\\bliggûh\\b", "leggûh"},
      new []{ "\\b(L|l)igge\\b", "$1egge" },
      new []{ "\\b(L|l)igt\\b", "$1eg" },
      new []{ "(?<![p])(L|l)ez", "$1eiz"}, // 'lezer', geen 'plezierig'
      new []{ "lf", "lluf"}, // 'zelfde'
      new []{ "ll([ ,.])", "l$1" }, // 'till'
      new []{ "(a|e|i|o|u)rk\\b", "$1rrek" }, // 'park', 'stork'
      new []{ "(P|p)arke", "$1agke"}, // 'parkeervergunning', moet voor '-ark'
      new []{ "ark(?![a])", "arrek" }, // 'markt', 'marktstraat', geen 'markante'
      new []{ "ark", "agk"}, // 'markante', moet na -ark
      new []{ "\\b(M|m)oet\\b", "$1ot"}, // 'moet', geen 'moeten'
      new []{ "nair", "nèâh" }, // 'culinair'
      new []{ "neme\\b", "neime" }, // 'nemen', geen 'evenementen''
      new []{ "nce", "nse"}, // 'nuance'
      new []{ "\\b(N|n)u\\b", "$1âh"},
      new []{ "ny", "ni" }, // 'hieronymus' 
      new []{ "\\bmad", "med"}, // 'madurodam'
      new []{ "oeder", "oedâhr" }, // 'bloederigste'
      new []{ "(?<![v])(a|o)(rdt|rd)(?![eû])", "$1gt"}, // wordt, word, hard, geen 'worden', 'wordûh', 'boulevard'
      new []{ "ord(e|û)", "ogd$1"}, // 'worden'
      new []{ "(N|n)(|o)od", "$1aud"}, // 'noodzakelijk'
      new []{ "nirs\\b", "nieâhs" }, // 'souvenirs'
      new []{ "l(f|k|m|p)(?![au])", "lle$1"}, // 'volkslied', 'behulp', geen 'elkaar', 'doelpunten'
      new []{ "olk", "ollek"}, // 'volkslied'
      new []{ "(F|f)olleklore", "$1olklore" },
      new []{ "o(c|k)a", "auka" }, // 'locaties'
      new []{ "(?<![o])oms", "omps" }, // 'aankomsthal'
      new []{ "one(e|i)", "aunei" }, // 'toneel'
      new []{ "oni", "auni" }, // 'telefonische'
      new []{ "hore", "hoâhre"}, // 'bijbehorende'
      new []{ "org(?![i])", "orrag"}, // 'zorg', geen 'orgineel'
      new []{ "orp", "orrep"}, // 'ontworpen'
      new []{ "mor\\b", "moâh"}, // 'humor', geen 'humoristische'
      new []{ "\\borg", "oâhg"}, // 'orgineel'
      new []{ "Over(?![ei])", "Auvâh"}, // 'overgebleven', 'overnachten', geen 'overeenkomsten', 'overige'
      new []{ "(?<![z])over(?![ei])", "auvâh"}, // 'overgebleven', geen 'overeenkomsten', 'overige', 'zover'
      new []{ "o(v|z)e", "au$1e"},
      new []{ "(?<![g])o(b|d|g|k|l|m|p|s|t|v)(i|e)", "au$1$2"}, // 'komen', 'grote', 'over', 'olie', 'notie', geen 'gokje'
      new []{ "O(b|d|g|k|l|m|p|s|t|v)(i|e)", "Au$1$2"}, // zelfde, maar dan met hoofdletter
      new []{ "\\bout", "âht" }, // 'outdoor'
      new []{ "\\bOut", "Âht" }, // 'Outdoor'
      new []{ "\\b(V|v)er\\b", "$1eâh"}, // 'ver'
      new []{ "(D|d)ert","$1eâht"}, // 'dertig'
      new []{ "der(?![deianrouèt])", "dâh"},// 'moderne'/'moderrene', geen 'dertig', 'derde'
      new []{ "\\b(P|p|T|t)er\\b", "$1eâh" }, // 'per', 'ter'
      new []{ "(Z|z)auver\\b", "$1auveâh" }, // 'zover'
      new []{ "(?<![ io])er\\b", "âh" }, // 'kanker', geen 'hoer', 'er', 'per', 'hier' , moet voor 'over' na o(v)(e)
      new []{ "(P|p)er(?!i)", "$1âh"}, // 'supermarkt', geen 'periode', moet na 'per'
      new []{ "(P| p)o(^st)" , "$1au$2"}, // 'poltici'
      new []{ "p ik\\b", "ppik"}, // 'hep ik'
      new []{ "ppen", "ppe" }, // 'poppentheater'
      new []{ "popu", "paupe"}, // 'populairste'
      new []{"(p|P)ro(?![oefkns])", "$1rau" }, // 'probleem', geen 'prof', 'prostituee', 'instaprondleiding'
      new []{ "(p|P)rofe", "$1raufe" }, // 'professor', 'professioneel'
      new []{ "ersch", "esch"}, // 'verschijn'
      new []{ "(A|a)rme", "$1rreme"}, // 'arme'
      new []{ "re(s|tr)(e|o)", "rei$1$2"}, // 'resoluut', 'retro', 'reserveren'
      new []{ "palè", "pelè"}, // voor Vredespaleis
      new []{ "(R|r)elax", "$1ieleks" },
      new []{ "(R|r)estâhrant", "$1esterant"},
      new []{ "rants\\b", "rans"}, // 'restaurants'
      new []{ "rigste", "ragste"}, // 'bloederigste'
      new []{ "rod", "raud"}, // 'madurodam'
      new []{ "(r|R)ou", "$1oe"}, // 'routes'
      new []{ "(a|o)rt", "$1gt"},  //'korte' 
      new []{ "([Rr])o(?=ma)" , "$1au"}, // voor romantisch, maar haal bijv. rommel eruit
      new []{ "inds", "ins"}, // 'sinds'
      new []{ "seque", "sekwe"}, // 'inconsequenties'
      new []{ "sjes\\b", "ssies"}, // 'huisjes'
      new []{ "(S|s)hop", "$1jop" }, // 'shop'
      new []{ "stje\\b", "ssie"}, // 'beestje'
      new []{ "st(b|d|g|h|j|k|l|m|n|p|v|w|z)", "s$1"}, // 'lastpakken', geen 'str'
      new []{ "(S|s)ouv", "$1oev" }, // 'souvenirs'
      new []{ "(?<![gr])st\\b", "s"}, // 'haast', 'troost', 'gebedsdienst', geen 'barst'
      new []{ "tep\\b", "teppie"}, // 'step'
      new []{ "té\\b", "tei"}, // 'satè'
      new []{ "tion", "sion"}, // 'station'
      new []{ "(d|t)je\\b", "$1sje"}, // 'biertje', 'mandje'
      new []{ "\\b(T|t)o\\b", "$1oe"}, // to
      new []{ "(p|t)o\\b", "$1au"}, // 'expo', moet na 'au'/'ou'
      new []{ "toma", "tauma"}, // moet na 'au'/'ou', 'automatiek', geen 'tom'
      new []{ "(T|t)ram", "$1rem"}, // 'tram'
      new []{ "quaran", "karre" }, // 'quarantaine', moet voor 'ua'
      new []{ "ua", "uwa"}, // 'nuance', 'menstruatie'
      new []{ "(J|j)anu", "$1anne" }, // 'januari', moet na 'ua'
      new []{ "ùite\\b", "ùitûh" }, // 'buiten'
      new []{ "(u|ù)igt\\b", "$1ig"}, // 'zuigt'
      new []{ "(U|u)ren", "$1re"}, // 'uren'
      new []{ "ùidâh\\b", "ùiâh"}, // 'klokkenluider'
      new []{ "unch", "uns" }, // 'lunch'
      new []{ "(?<![u])urg", "urrag"}, // 'Voorburg', geen natuurgras
      new []{ "(?<![u])urs", "ugs" }, // 'excursies', geen 'cultuurschatten'
      new []{ "uur", "uâh" }, // 'literatuurfestival', moet voor '-urf'
      new []{ "ur(f|k)", "urre$1"}, // 'Turk','snurkende','surf'
      new []{ "(T|t)eam", "$1iem"},
      new []{ "tu(?![âfkust])", "te"}, // 'culturele', geen 'tua', 'vintage', 'instituut', 'tussenletter', 'stuks'
      new []{ "\\bvan je\\b", "vajje"},
      new []{ "\\bvan (het|ut)\\b", "vannut"},
      new []{ "([Vv])er(?![aeious])", "$1e"}, // wel 'verkoop', geen 'verse', 'veranderd', 'overeenkomsten', 'overige'
      new []{ "([Vv])ersl", "$1esl"}, // 'verslag'
      new []{ "\\bvaka", "veka"}, // 'vakantie'
      new []{ "vaka", "veka" }, // 'vakantie' 
      new []{ "vard\\b", "vâh"}, // 'boulevard'
      new []{ "\\b(V|v)ege", "$1eige"}, // 'vegetarisch'
      new []{ "voetbal", "foebal"},
      new []{ "we er ", "we d'r "}, // 'we er'
      new []{ "\\ber\\b", "d'r"},
      new []{ "\\bEr\\b", "D'r"},
      new []{ "(I|i)n je\\b", "$1jje"},
      new []{ "wil hem\\b", "wil 'm"},
      new []{ "(W|w|H|h)ee(t|l)", "$1ei$2"}, // 'heel', 'heet'
      new []{ "yo", "yau" }, // 'yoga' 
      new []{ "\\b(Z|z)ee", "$1ei"}, // 'zeeheldenkwartier'
      new []{ "eep", "iep"}, // 'keeper'
      new []{ "\\b(Z|z)ult\\b", "$1al"},
      new []{ "z'n" , "ze"}, // 'z'n'
      new []{ "\\bzich\\b", "ze ège"}, // 'zich'
      new []{ "z(au|o)'n", "zaun"},
      new []{ "\\bzegt\\b", "zeg"},
      new []{ "(z|Z)(o|ó)(?![cenr])", "$1au"}, // 'zogenaamd', 'zó', geen 'zoeken', 'zondag', 'zorgen', 'zocht'
      new []{ "'t", "ut"},
      new []{ "derr", "dèrr" }, // 'moderne, moet na 'ern'/'ode'
      new []{ "Nie-westegse", "Niet westagse" },
      new []{ "us sie", "us-sie" }, // 'must see'
      new []{ "\\bThe Hague\\b", "De Heek" }, // moet na 'ee -> ei'
      new []{ "Krowne", "Kraun" },
      new []{ "social media", "sausjel miedieja" }, // moet na 'au'
      new []{ "aine", "ène" }, // 'quarantaine'
      new []{ "(?<![eèin])gel\\b", "sjel"}, // 'handgel', geen 'regel', 'pingel'
      new []{ "ingel\\b", "ingol"}, // 'pingel'
      new []{ "down", "dâhn" }, // 'lockdown'
      new []{ "lock", "lok"}, // 'lockdown'
      new []{ "(?<![s])sis", "sus"}, // 'crisis', geen 'rassis'van 'racist'
      new []{ "COVID", "KAUVID"}, // 'COVID-19'

      //quick fixups
      new [] { "stgong>", "strong>"}, //fixups for <strong tag>
      new [] { "kute;", "cute;" }, // fixups for &eacute; tag
      new [] { "&ksedil;", "&ccedil;" }, // fixups for &ccedil; tag
      new [] { "lie>", "li>" } // fixups for <li> tag
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
      var copyValue = string.Copy(dutch);
      return TranslationReplacements.Aggregate(copyValue, (current, replacement) => Regex.Replace(current, replacement[0], replacement[1], RegexOptions.CultureInvariant));
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
        return Enumerable.Empty<Tuple<string, bool>>();

      var result = new List<Tuple<string, bool>>();
      var haags = dutch;
      foreach (var replacement in TranslationReplacements)
      {
        var original = haags;
        haags = Regex.Replace(original, replacement[0], replacement[1], RegexOptions.CultureInvariant);

        result.Add(Tuple.Create(replacement[0], !original.Equals(haags, StringComparison.InvariantCultureIgnoreCase)));
      }
      return result;
    }
#endif
  }
}
