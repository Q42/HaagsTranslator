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
      new []{ "Uitkijktoren", "Kèkstègâh"},
      new []{ "uitkijktoren", "kèkstègâh"},
      new []{ "childerswijk", "childâhswijk"},
      new [] { "eider", "èijâh" }, // 'projectleider'
      new []{ "(?<![o])ei", "è"}, // moet voor 'scheveningen' en 'eithoeke', geen 'groeit'
      new []{ "Ei", "È"}, // 'Eind'
      new []{ "(B|b)roodje bal", "$1eschùitstùitâh"},
      new []{ "koets", "patsâhbak"},
      new []{ "kopje koffie", "bakkie pleuâh" },
      new []{ "Kopje koffie", "Bakkie pleuâh" },
      new []{ "koffie\\b", "pleuâh" },
      new []{ "Koffie\\b", "Pleuâh" },
      new []{ "Kurhaus", "Koeâhhâhs"}, // moet voor 'au' en 'ou'
      new []{ "\\bMaurice\\b", "Mâhpie"}, // moet voor 'au' en 'ou'
      new []{ "Hagenezen", "Hageneize"}, // moet na 'ei'
      new []{ "toiletten", "pleis"},
      new []{ "Toiletten", "Pleis"},
      new []{ "toilet", "plei"},
      new []{ "Toilet", "Plei"},
      new []{ "(B|b)al gehakt", "$1eschùitstùitâh"},
      new []{ "(N|n)erd", "$1euâhd"},
      new []{ "(L|l)unchroom", "$1unsroem" },
      new []{ "\\bThis\\b", "Dis" },
      new []{ "design", "diesain" },
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
      new []{ "(?<![vZz])(O|o)r(i|)g", "$1âhg" }, // 'originele', 'organisatie', geen 'zorg', 'vorige'
      new []{ "orige", "orage" }, // 'vorige'
      new []{ "chine", "sjine" }, // 'machine'
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
      new []{ "you(?![n])", "joe" }, // geen 'young'
      new []{ "You(?![n])", "Joe" }, // geen 'young'
      new []{ "(W|w)eekend", "$1iekend" },
      new []{ "(W|w)ork", "$1urrek" },
      new []{ "(B|b)ibliotheek", "$1iebeleteik" },
      new []{ "(F|f)ood", "$1oet"},
      new []{ "mondkapje", "bekbedekkâh"},
      new []{ "Mondkapje", "Bekbedekkâh"},
      new []{ "(D|d)evelop", "$1ievellep"},
      new []{ "doe je het", "doejenut"},
      new []{ "\\bsee\\b", "sie"}, // van 'must see'
      new []{ "(M|n|R|r)ust(?![ai])", "$1us" }, // van 'must see', 'rustsignaal', geen 'rustig'
      new []{ "(M|m)oeten", "$1otte"}, // moet voor '-en'
      new []{ "(w|W)eleens", "$1elles"}, // 'weleens', moet voor 'hagenees'
      new []{ "(g|G)ouv", "$1oev"}, // 'gouveneur'
      new []{ "(H|h)eeft", "$1ep"}, // 'heeft', moet voor 'heef'
      new []{ "(on|i)der(?!e)", "$1dâh" }, // 'onder', 'Zuiderpark', geen 'bijzondere'
      new []{ "iendel", "iendâhl" }, // 'vriendelijk'
      new []{ "(?<![ao])ndere", "ndâhre" }, // 'bijzondere', geen 'andere'
      new []{ "uier\\b", "uiâh" }, // 'sluier', moet voor 'ui'
      new []{ "ui", "ùi"}, // moet voor 'ooi' zitte n
      new []{ "Ui", "Ùi" },
      new []{ "(?<![ieopfv])ert\\b", "egt" }, // 'gert', geen 'viert', 'expert', 'levert'
      new []{ "pert\\b", "peâh" }, // 'expert'
      new []{ "\\b(V|v)ert", "$1et"}, // 'vertegenwoordiger', moet voor '-ert'
      new []{ "(?<![eo])erte", "egte" }, // 'concerten'
      new []{ "(?<![eo])(a|o)r(|s)t(?!j)", "$1g$2t" }, // barst, martin etc., geen 'eerste', 'biertje', 'sport', 'voorstellingen'
      new []{ " er aan", " d'ran"}, // 'er aan'
      new []{ "(A|a)an het\\b", "$1nnut"}, // 'aan het', moet voor 'gaan'
      new []{ "\\b(A|a)an", "$1n" }, // 'aan', 'aanrennen'
      new []{ "\\b(G|g)aan\\b", "$1an" }, // 'gaan'
      new []{ "(H|h)oud\\b", "$1ou"}, // 'houd', moet voor 'oud'
      new []{ "(B|b|R|r|P|p)ou(l|t)", "$1oe$2"}, // 'boulevard', 'routes', 'poule'
      new []{ "oele\\b", "oel"}, // 'poule'
      new []{ "(au|ou)w(?!e)", "$1"}, // 'vrouw', ''flauw', maar zonder 'blauwe'
      new []{ "oude", "ouwe"}, // 'goude'
      new []{ "\\b(T|t)our\\b", "$1oeâh"},
      new []{ "diner\\b", "dinei"},
      new []{ "o(e|u)r\\b", "oeâh"}, // 'broer', 'retour', moet voor 'au|ou'
      new []{ "oer(?![aieou])", "oeâh"}, // 'beroerd', 'hoer', geen 'toerist', 'stoere, moet voor 'au|ou'
      new []{ "(?<![epYy])(au|ou)(?![v])", "âh" }, // 'oud', geen 'souvenirs', 'cadeau', 'bureau', 'routes', 'poule', 'young'
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
      new []{ "(am|at|ig|ig|it|kk|nn)en(?![ e,.?!])", "$1e"}, // 'en' in een woord, bijv. 'samenstelling', 'eigenlijk', 'buitenstaander', 'statenkwartier', 'dennenweg', 'klokkenluider', geen 'betrokkenen'

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
      new []{ "\\bApp", "Ep"},
      new []{ "\\bapp", "ep"},
      new []{ "(o|u)als\\b", "$1as"}, // als, zoals
      new []{ "(b|k|w)ar\\b", "$1âh"},
      new []{ "\\b(A|a)ls\\b", "$1s"},
      new []{ " bent\\b", " ben"}, // 'ben', geen 'instrument'
      new []{ "bote", "baute"}, // 'boterham'
      new []{"(B|b)roc", "$1rauc" }, // 'brochure'
      new []{ "bt\\b", "b"}, // 'hebt'
      new []{ "cce", "kse"}, // 'accenten', geen 'account'
      new []{ "cc", "kk"}, // 'account'
      new []{ "chique", "sjieke" },
      new []{ "(?<![s])chure", "sjure" }, // 'brochure', geen 'schuren'
      new []{ "pact", "pekt"}, // 'impact'
      new []{ "aca", "akke"}, // 'vacatures'
      new []{ "c(a|o|t|r)", "k$1"}, // 'geactualiseerde', 'directie', 'crisis'
      new []{ "C(a|o|t|r)", "K$1" }, // 'Concerten', 'Cadeau'
      new []{ "(c|k)or\\b", "$1oâh" }, // 'decor'
      new []{ "(?<![.])c(a|o)", "k$1" }, // 'concerten', 'cadeau', 'collectie', geen '.com'
      new []{ "\\bkoro", "kerau"}, // 'corona'
      new []{ "cu", "ku" }, // 'culturele'
      new []{ "Cu", "Ku" }, // 'culturele'
      new []{ "ci(ë|ee)l", "sjeil" }, // 'financieel', 'financiële'
      new []{ "(ch|c|k)t\\b", "$1"}, // woorden eindigend op 'cht', 'ct', 'kt', of met een 's' erachter ('geslachts')
      new []{ "(ch|c|k)t(?![aeiouâr])", "$1"}, // woorden eindigend op 'cht', 'ct', 'kt', of met een 's' erachter ('geslachts'), geen 'elektronische'
      new []{ "(?<![Gg])(e|r)gt\\b", "$1g"}, // 'zorgt', 'legt'
      new []{ "(d|D)at er", "$1attâh"}, // 'dat er'
      new []{ "(d|D)at is ", "$1a's "}, // 'dat is'
      new []{ "denst", "dest" },
      new []{ "derb", "dâhb"},
      new []{ "nderh", "ndâhh"}, // 'anderhalf', geen 'derhalve'
      new []{ "derd\\b", "dâhd"}, // 'veranderd'
      new []{ "(D|d)eze(?![l])", "$1eize"}, // 'deze', geen 'dezelfde'
      new []{ "dt\\b", "d"}, // 'dt' op het einde van een woord
      new []{"\\b(B|b)ied\\b", "$1iedt" }, // uitzondering, moet na '-dt'
      new []{ "(D|d)y", "$1i"}, // dynamiek
      new []{ "eaa", "eiaa"}, // 'ideaal'
      new []{ "(E|e)nig", "$1inag"}, // 'enigste'
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
      new []{ "\\been\\b",    "un"},
      new []{ "\\bEen\\b", "Un"},
      new []{ " eens", " 'ns"}, // 'eens'
      new []{ "(?<![eo])erd\\b", "egd"}, // 'werd', geen 'verkeerd', 'gefeliciteerd', 'beroerd
      new []{ "eerd", "eâhd"}, // 'verkeerd'
      new []{ "(?<![k])ee(d|f|g|k|l|m|n|p|s|t)", "ei$1"}, // 'bierfeest', 'kreeg', 'greep', geen 'keeper'
      new []{ "(?<![l])ands\\b", "ens"}, // 'hands', moet voor 'ds', geen 'Nederlands'
      new []{ "(?<![st])ain", "ein"}, // 'trainer', geen 'quarantaine', 'design'
      new []{ "(?<![èijhmr])ds(?![ceèt])", "s" }, // moet na 'ee','godsdienstige', 'gebedsdienst', geen 'ahdste', 'boodschappen', 'beroemdste', 'eigentijds', 'weidsheid', 'reeds', 'strandseizoen', 'wedstrijd', 'nerds'
      new []{ "(?<![eh])ens\\b", "es"}, // 'ergens', geen 'weleens', 'hands/hens'
      new []{ "kens", "kes"}, // 'Valkenswaard'
      new []{ "(D|d)ance", "$1ens" }, // moet na '-ens' 
      new []{ "(?<![ hi])eden\\b", "eije"}, // geen 'bieden'. 'bezienswaardigheden'
      new []{ "(?<![ bgi])eden", "eide" }, // 'hedendaagse', geen 'bedenken'
      new []{ "\\b(E|e)ve", "$1ive" }, // 'evenementen'
      new []{ "(?<![a])(m|M|R|r)e(d|t|n)e(?![ei])", "$1ei$2e"}, // 'medeklinkers', 'rede', geen 'Hagenees', 'meneer', geen 'meteen'
      new []{ "(G|g)ener", "$1einer"}, // 'generatie'
      new []{ "(E|e)nerg", "$1inerg"}, // 'energie'
      new []{ "eugd", "eug" }, // 'jeugd', 'jeugdprijs'
      new []{ "(?<![o])epot\\b", "eipau"}, // 'depot'
      new []{ "(e|E)rg\\b", "$1rrag"}, // 'erg', moet voor 'ergens'
      new []{ "(?<![fnN])(a|o)rm","$1rrem" }, // 'platform', 'vormgeving', 'warm', geen 'normale', 'informatie'
      new []{ "(f|N|n)orma", "$1oâhma" }, // 'normale', 'informatie', geen 'boorplatform'
      new []{ "(i|I|a|A)(l|n)terna", "$1$2tâhna" }, // 'internationale', 'alternatieve'
      new []{ "elden", "elde"}, // 'zeeheldenkwartier'
      new []{ "oeter", "oetâh" }, // 'Zoetermeer', moet voor 'kermis'
      new []{ "erm\\b", "errum"}, // 'scherm', moet voor 'kermis'
      new []{ "(?<![ iegoptvV])er(m|n)", "erre$1"}, // kermis', geen 'vermeer', 'vermoeide', 'toernooi', 'externe', 'supermarkt', termijn, 'hierna', 'tijgermug'
      new []{ "(?<![edktv])(e|E)rg(?!ez|i)", "$1rreg"}, // 'kermis', 'ergens', geen 'achtergelaten', 'neergelegd', 'overgebleven', 'ubergezellige', 'bekergoal', 'energie', 'rundergehakt'
      new []{ "(?<![i])(g|b)er(?![eoiuaâè])", "$1âh"}, // 'tijgermug', 'ubergezellige', moet na '-erg', geen 'beraden', 'vertegenwoordiger'
      new []{ "epers\\b", "epâhs"}, // 'developers'
      new []{ "(P|p)ers(?![l])", "$1egs"}, // 'pers', geen 'superslimme'
      new []{ "(K|k)erst", "$1egs"}, // 'kerstfeest'
      new []{ "(?<![e])(t|V|v)ers(?![clt])", "$1egs"}, // 'vers', 'personeel', 'versie', 'diverse', geen 'gevers', 'verscheen', 'eerste', 
      new []{ "(G|g)eve(r|n)", "$1eive$2" }, // 'Gevers', moet na 'vers', geen 'gevestigd'
      new []{ "(t|w|W)ene", "$1eine" }, // 'stenen', 'wenen'
      new []{ "renstr", "restr" }, // 'herenstraat' (voor koppelwoorden)
      new []{ "(?<![eIio])eder", "eider" }, // 'Nederland', geen 'iedereen', 'bloederige', 'Iedere'
      new []{ "(?<![eiop])ers\\b", "âhs"}, // 'klinkers', geen 'pers', 'personeel'
      new []{ "(H|h)er(?![erop])", "$1eâh" }, // 'herzien', 'herstel', geen 'herenstraat', 'heroische', 'scherm', 'scherp', moet voor 'ers'
      new []{ "\\b(K|k)ers", "$1egs"}, // 'kerstfeest'
      new []{ "(?<![vVi])ers(c|t)", "âhs$1"}, // 'eerste', 'bezoekerscentrum', geen 'verschaffen', 'Verstappen', 'deurkierstandhouder'
      new []{ "erwt", "erret" }, // 'erwtensoep'
      new []{ "(?<![eor])eci", "eici" }, // 'speciaal', geen 'precies'
      new []{ "eserve(e|r)", "eiserve$1" }, // 'reserveer', 'reserveren'
      new []{ "erve\\b", "erreve" }, // 'reserve',
      new []{ "ervek", "errevek" }, // 'reservekeeper',
      new []{ "serv", "sev" }, // 'reserveren',
      new []{ "eiser", "eisâh"}, // 'reserveer'
      new []{ "eur\\b", "euâh"}, // worden eindigend op 'eur', zoals 'deur', 'gouveneurlaan', geen 'kleuren'
      new []{ "eur(?![eio])", "euâh"}, // worden eindigend op 'eur', zoals 'deur', 'gouveneurlaan', geen 'kleuren', 'goedkeuring', 'euro
      new []{ "eur(i|o)", "euâhr$1"}, // 'goedkeuring', 'euro'
      new []{ "eurl", "euâhl"}, // worden eindigend op 'eur', zoals 'deur', 'gouveneurlaan'
      new []{ "eer", "eâh" }, // 'zweer', 'neer'
      new []{ "elk\\b", "ellek"}, // 'elk'
      new []{ "(?<![og])ega", "eige"}, // 'negatief', geen 'toegang', 'gegaan'
      new []{ "(E|e)xt", "$1kst" }, // 'extra'
      new []{ "(H|h|n|T|t)ele", "$1eile"}, // 'gehele', 'hele', 'originele', 'televisie'
      new []{ "\\b(E|e)le", "$1ile"}, // 'elektronica'
      new []{ "ffis", "ffes"}, // 'officiele'
      new []{ "(G|g)ese", "$1esei"}, // 'geselecteerd'
      new []{ "\\b(g|G|v|V)ele\\b", "$1eile" }, // 'vele', 'gele', 'hele'
      new []{ "ebut", "eibut" }, // 'debuteren', geen 'debuut'
      new []{ "\\b(D|d)elen", "$1eile"}, // 'delen', geen 'wandelen'
      new []{ "sdelen", "sdeile"}, // 'geslachtsdelen', geen 'wandelen'
      new []{ "(?<![diokrs])ele(n|m|r)", "eile$1"}, // 'helemaal', geen 'enkele', 'winkelen', 'wandelen', 'borrelen', 'beginselen'
      new []{ "(B|b)eke(?![n])", "$1eike"}, // 'beker', geen 'bekende'
      new []{ "(B|b)ene(?![v])", "$1eine" }, // 'benen', geen 'beneveld'
      new []{ "(?<![ioBbg])eke(?![e])", "eike"}, // geen 'aangekeken' op 'gek', wel 'kek', 'reservekeeper'
      new []{ "(?<![r])rege(?![r])", "reige" }, // 'gekregen', geen 'berrege', 'regeren'
      new []{ "(T|t)ege(?![l])", "$1eige" }, // 'tegen', geen 'tegelijkertijd'
      new []{ "(?<![bBIiort])e(g|v|p)e(l|n|m| )", "ei$1e$2" }, // aangegeven, 'leverde', geen 'geleden', 'uitspreken', 'geknepen', 'goeveneur', 'verdiepen', 'postzegels', 'begeleiding', 'berregen', 'tegelijkertijd'
      new []{ "(?<![e])dige", "dege"}, // 'vertegenwoordiger', moet na 'ege', geen 'volledige'
      new []{ "(L|l)ever", "$1eiver"}, // 'leverde'
      new []{ "alve\\b", "alleve"}, // 'halve', moet na 'aangegeven'
      new []{ "\\b(K|k)en\\b", "$1an"}, // moet voor -en
      new []{ "(a|o)ien\\b", "$1ie"}, // 'uitwaaien', geen 'zien'
      new []{ "(?<![ bieo])en([.?!])", "ûh$1"}, // einde van de zin, haal ' en ', 'doen', 'zien' en 'heen'  eruit
      new []{ "(?<![ ])ben([.?!])", "ûh$1"}, // geen 'ben'
      new []{ "(?<![ bieohr])en\\b", "e"}, // haal '-en' eruit, geen 'verscheen', 'tien', 'indien', 'ben', 'doen', 'hen'
      new []{ "(?<![r])ren\\b", "re"}, // oren, geen 'kerren'
      new []{ "bben\\b", "bbe"}, // 'hebben'
      new []{ "oien\\b", "oie"}, // 'weggooien'
      new []{ "enso", "eso" }, // 'erwtensoep'
      new []{ "eum", "eijum" }, // 'museum'
      new []{ "(?<![eio])enm(?![e])", "em" }, // 'kinderboekenmuseum', geen 'kenmerken'
      new []{ "(?<![eiorRvV])en(b|h|j|l|p|r|v|w|z)", "e$1"}, // 'binnenhof', geen 'paviljoenhoeder', 'venlo', 'Bernhard'
      new []{ "([Hh])eb je ", "$1ebbie "}, // voor '-eb'
      new []{ "(H|h)eb (un|een)\\b", "$1ep'n"}, // voor '-eb'
      new []{ "(?<![eu])eb\\b", "ep"},
      new []{ "(E|e)x(c|k)", "$1ksk" }, // 'excursies'
      new []{ "(?<![ s])teri", "tâhri" }, // 'karakteristieke'
      new []{ "(?<![ sS])ter(?![aeirnû])", "tâh"}, // 'achtergesteld', geen 'beluisteren', 'literatuur', 'sterren', 'externe', 'sterker', 'karakteristieke'
      new []{ "sterd", "stâhd"}, // 'Amsterdam'
      new []{ "(F|f)eli", "$1eili" }, // 'gefeliciteerd'
      new []{ "(F|f)en(a|o)", "$1ein$2" }, // 'fenomenale'
      new []{ "(I|i)ndeli", "$1ndeili" }, // 'indeling', geen 'eindelijk', 'wandelingen'
      new []{ "(f|p)t\\b", "$1"}, // 'blijft', 'betrapt'
      new []{ "\\b(N|n)iet\\b", "$1ie" }, // 'niet', geen 'geniet'
      new []{ "fd(?![eo])", "f"}, // 'hoofd', 'hoofdtrainer', geen 'zelfde', 'verfdoos'
      new []{ "(F|f)eb", "$1eib" }, // 'februari'
      new []{ "ngt\\b", "nk"}, // 'hangt'
      new []{ "(?<![o])e(k|v)ing", "ei$1ing"}, // 'omgeving', 'onderbreking', geen 'boeking'
      new []{ "gje\\b", "ggie"}, // 'dagje'
      new []{ "go\\b", "gau" }, // 'logo'
      new []{ "go(r)", "gau$1" }, // 'algoritme'
      new []{ "gelegd\\b", "geleige"}, // 'neergelegd'
      new []{ "([HhVvr])ee(l|n|t)", "$1ei$2"}, // 'verscheen', 'veel', 'overeenkomsten', 'heet'
      new []{ "(I|i)n het", "$1nnut"}, // 'in het'
      new []{ "(K|k)ete", "$1eite"}, // 'ketel'
      new []{ "\\b(E|e)te", "$1ite"}, // 'eten', 
      new []{ "(?<![ior])ete(?![il])", "eite" }, // 'hete', 'gegeten', geen 'bibliotheek','erretensoep', 'koffietentjes', 'genieten, 'roetes (routes)', 'vertelde'
      new []{ "(d|h|l|m|r|t)ea(?![m])", "$1eija" }, // 'theater', geen 'team'
      new []{ "\\bhet\\b", "ut"},
      new []{ "Het\\b", "Ut"},
      new []{ "(?<![eouù])i\\b", "ie" }, // 'januari'
      new []{ "ieri(n|g)", "ieâhri$1"}, // 'plezierig', 'viering'
      new []{ "emier", "emjei" }, // 'premier'
      new []{ "(?<![uù])ier(?!(a|e|i|ony))", "ieâh" }, // 'bierfeest', 'hieronder', geen 'hieronymus', 'plezierig', 'dieren', 'sluier'
      new []{ "iero(?!e|o|nd)", "ierau" }, // 'hieronymus', geen 'hieronder'
      new []{ "ière", "ijerre"}, // 'barriere'
      new []{ "ibu", "ibe"}, // 'tribunaal'
      new []{ "icke", "ikke" }, // 'tickets'
      new []{ "iti(a|o|au)", "isi$1" }, // 'initiatief', 'traditioneel'
      new []{ "ijgt\\b", "ijg"}, // 'krijgt', moet voor 'ij\\b'
      new []{ "(B|b)ijz", "$1iez" }, // 'bijzondere', moet voor 'bij'
      new []{ "ij\\b",  "è"}, // 'zij', 'bij'
      new []{ "(?<![e])ije(?![ei])", "èje" }, // 'bijenkorf', 'blije', geen 'geleie', 'bijeenkomst'
      new []{ "èjen", "èje" }, // 'bijenkorf'
      new []{ "(B|b)ij", "$1è" }, // 'bijbehorende'
      new []{ "\\blijk\\b", "lèk" }, // 'lijk' , geen 'eindelijk' ('-lijk')
      new []{ "(D|d|K|k|p|R|r|W|w|Z|z)ijk", "$1èk"}, // 'wijk', geen '-lijk'
      new []{ "ij([dgslmnftpvz])",  "è$1"}, // 'knijp', 'vijver', 'stijl', 'vervoersbewijzen', geen '-lijk'
      new []{ "edig", "eidag"}, // 'volledige'
      new []{ "(?<![euù])ig\\b", "ag"}, // geen 'kreig', 'vliegtuig'
      new []{ "tigdù", "tagdù"}, // 'vijftigduizend'
      new []{ "lige\\b", "lage"}, // 'gezellige'
      new []{ "(?<![euù])igd\\b", "ag" }, // gevestigd
      new []{ "\\bIJ", "È" }, // 'IJsselmeer'
      new []{ "ilm", "illem" }, // 'film'
      new []{ "ilieu", "ejui"}, // 'milieu'
      new []{ "incia", "insja"}, // 'provinciale'
      new []{ "inc(k|)(?![i])", "ink"}, // 'incontinentie', 'binckhorst', geen 'provinciale'
      new []{ "io(?![oen])", "iau"}, // 'audio', geen 'viool', 'station'
      new []{ "\\bin m'n\\b", "imme"},
      new []{ "(n|r)atio", "$1asjau" }, // 'internationale'
      new []{ "io\\b", "iau" }, // 'audio', geen 'viool', 'station', 'internationale'
      new []{ "io(?![oen])", "iau" }, // 'audio', geen 'viool', 'station', 'internationale'
      new []{ "ir(c|k)", "irrek" }, // 'circus'
      new []{ "(?<![gr])ties\\b", "sies"}, // 'tradities', moet voor -isch, geen 'smarties'
      new []{ "oï", "aui" }, // 'heroische'
      new []{ "isch(|e)", "ies$1"},
      new []{ "is er", "istâh"},
      new []{ "ap je\\b", "appie" }, // 'stap je'
      new []{ "(p) je\\b", "$1ie" }, // 'loop je', geen 'stap je'
      new []{ "(?<![i])(g|k) je\\b", "$1$1ie" }, // 'zoek je', geen 'ik je'
      new []{ "jene", "jenei"}, // 'jenever'
      new []{ "jezelf", "je ège"}, // "jezelf"
      new []{ "(?<![oe])kje\\b", "kkie"}, // 'bakje', moet voor algemeen regel op 'je', TODO, 'bekje'
      new []{ "olg", "olleg"}, // 'volgens'
      new []{ "(a|i|o)(k|p)je\\b", "$1$2$2ie"}, // 'kopje', 'gokje', 'tipje', 'stapje'
      new []{ "(?<![ deèijstn])je\\b", "ie"}, // woorden eindigend op -je', zonder 'asje', 'rijtje', 'avondje', geen 'mejje' 'blèjje', 'skiën', 'oranje'
      new []{ "(K|k)an\\b", "$1en"}, // 'kan', geen 'kans', 'kaneel'
      new []{ "(K|k)unne", "$1enne"}, // 'kunnen', TODO, wisselen van u / e
      new []{ "(K|k)unt", "$1en" },
      new []{ "(K|k)led(?![d])", "$1leid"}, // 'kleding', geen 'kledder'
      new []{ "\\bOra", "Aura" }, // 'Oranje'
      new []{ "\\bora", "aura" }, // 'oranje'
      new []{ "orf", "orref" },
      new []{ "oro(?![eo])", "orau" }, // 'Corona'
      new []{ "Oo([igkm])", "Au$1" }, // 'ook' 
      new []{ "oo([difgklmnpst])", "au$1"}, // 'hoog', 'dood'
      new []{ "lo\\b", "lau"}, // 'venlo'
      new []{ "([RrNn])ij", "$1è"}, // 'Nijhuis'
      new []{ "tieg", "sieg" }, // 'vakantiegevoel'
      new []{ "(?<![e])tie\\b",   "sie"}, // 'directie', geen 'beauty'
      new []{ "enties\\b", "ensies"}, // 'inconsequenties', geen 'romantisch'
      new []{ "er(f|p)(?![a])", "erre$1"}, // 'modeontwerper', 'scherp', 'verf', geen 'verpakking'
      new []{ "(b|B|k|K|m|L|l|M|p|P|t|T|w|W)erk", "$1errek" }, // 'kerk', 'werkdagen', geen 'verkeer'
      new []{ "(f|k)jes\\b", "$1$1ies" }, // 'plekjes'
      new []{ "(M|m)'n", "$1e"}, // 'm'n'
      new []{ "(M|m)ong", "$1eg"}, // 'mongool'
      new []{ "k mein(?![ut])", "k meint"}, // 'ik meen', moet na 'ee', geen 'menu', 'gemeentemuseum'
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
      new []{ "neme\\b", "neime" }, // 'nemen', geen 'evenementen'
      new []{ "nemi", "neimi" }, // 'onderneming'
      new []{ "nce", "nse"}, // 'nuance'
      new []{ "\\b(N|n)u\\b", "$1âh"},
      new []{ "ny", "ni" }, // 'hieronymus' 
      new []{ "\\bmad", "med"}, // 'madurodam'
      new []{ "(?<![v])(a|o)(rdt|rd)(?![eû])", "$1gt"}, // wordt, word, hard, geen 'worden', 'wordûh', 'boulevard'
      new []{ "ord(e|û)", "ogd$1"}, // 'worden'
      new []{ "(N|n)(|o)od", "$1aud"}, // 'noodzakelijk'
      new []{ "nirs\\b", "nieâhs" }, // 'souvenirs'
      new []{ "l(f|k|m|p)(?![aeiroul])", "lle$1"}, // 'volkslied', 'behulp', geen 'elkaar', 'doelpunten', 'IJsselmeer', 'vuilcontainer', 'spelprogramma', 'asielminister', 'schoolklas'
      new []{ "(a|e|o)lk(?![ao])", "$1llek"}, // 'volkslied','elke', 'Valkenswaard', geen 'elkaar', 'welkom'
      new []{ "(F|f)olleklore", "$1olklore" },
      new []{ "o(c|k)a", "auka" }, // 'locaties'
      new []{ "(?<![o])oms", "omps" }, // 'aankomsthal'
      new []{ "one(e|i)", "aunei" }, // 'toneel'
      new []{ "on(a|i)", "aun$1" }, // 'telefonische', 'gepersonaliseerde'
      new []{ "hore", "hoâhre"}, // 'bijbehorende'
      new []{ "org(?![i])", "orrag"}, // 'zorg', geen 'orgineel'
      new []{ "orp", "orrep"}, // 'ontworpen'
      new []{ "mor\\b", "moâh"}, // 'humor', geen 'humoristische'
      new []{ "\\borg", "oâhg"}, // 'orgineel'
      new []{ "Over(?![ei])", "Auvâh"}, // 'overgebleven', 'overnachten', geen 'overeenkomsten', 'overige'
      new []{ "(?<![z])over(?![ei])", "auvâh"}, // 'overgebleven', geen 'overeenkomsten', 'overige', 'zover'
      new []{ "o(v|z)e", "au$1e"},
      new []{ "olop", "ollop"}, // 'volop'
      new []{ "i(p|P)hone", "aaifaun"},
      new []{ "Iphone", "Aaifaun"},
      new []{ "emo(?![e])", "eimau" }, // 'emotie', 'democratie', geen 'vemoeide'
      new []{ "(?<![gz])o(b|d|g|k|l|m|n|p|s|t|v)(i|e|o|au)", "au$1$2"}, // 'komen', 'grote', 'over', 'olie', 'notie', geen 'gokje', 'foto', 'doneren', 'zone'
      new []{ "O(b|d|g|k|l|m|p|s|t|v)(i|e)", "Au$1$2"}, // zelfde, maar dan met hoofdletter
      new []{ "\\bout", "âht" }, // 'outdoor'
      new []{ "\\bOut", "Âht" }, // 'Outdoor'
      new []{ "\\b(V|v)er\\b", "$1eâh"}, // 'ver'
      new []{ "(D|d)ert(?![u])","$1eâht"}, // 'dertig', geen 'ondertussen'
      new []{ "\\b(D|d)er\\b","$1eâh"}, // 'der'
      new []{ "der(?![dehianrouèt])", "dâh"},// 'moderne'/'moderrene', geen 'dertig', 'derde', 'derhalve'
      new []{ "\\b(P|p|T|t)er\\b", "$1eâh" }, // 'per', 'ter'
      new []{ "(Z|z)auver\\b", "$1auveâh" }, // 'zover'
      new []{ "ergi", "egi" }, // 'energie'
      new []{ "(?<![ io])er\\b", "âh" }, // 'kanker', geen 'hoer', 'er', 'per', 'hier' , moet voor 'over' na o(v)(e)
      new []{ "eiker(g|h)", "eikâh$1" }, // 'bekergoal', 'bekerheld'
      new []{ "orm", "orrum" }, // 'platform'
      new []{ "(P|p)er(?![aeirt])", "$1âh"}, // 'supermarkt', geen 'periode', 'expert', 'beperkt/beperrekt', 'operaties', 'beperken/beperreken' moet na 'per'
      new []{ "(P| p)o(^st)" , "$1au$2"}, // 'poltici'
      new []{ "p ik\\b", "ppik"}, // 'hep ik'
      new []{ "ppen", "ppe" }, // 'poppentheater'
      new []{ "popu", "paupe"}, // 'populairste'
      new []{"(p|P)ro(?![oefkns])", "$1rau" }, // 'probleem', geen 'prof', 'prostituee', 'instaprondleiding'
      new []{ "(p|P)rofe", "$1raufe" }, // 'professor', 'professioneel'
      new []{ "ersch", "esch"}, // 'verschijn'
      new []{ "(A|a)rme", "$1rreme"}, // 'arme'
      new []{ "re(s|tr)o", "rei$1o"}, // 'resoluut', 'retro'
      new []{ "palè", "pelè"}, // voor Vredespaleis
      new []{ "(R|r)elax", "$1ieleks" },
      new []{ "(R|r)estâhrant", "$1esterant"},
      new []{ "(R|r)esu", "$1eisu"}, // 'resultaat'
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
      new []{ "stf", "sf"}, // 'kerstfeest'
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
      new []{ "anch", "ansj" }, // 'branch'
      new []{ "(?<![u])urg", "urrag"}, // 'Voorburg', geen natuurgras
      new []{ "(?<![u])urs", "ugs" }, // 'excursies', geen 'cultuurschatten'
      new []{ "uur", "uâh" }, // 'literatuurfestival', moet voor '-urf'
      new []{ "ur(f|k)", "urre$1"}, // 'Turk','snurkende','surf'
      new []{ "(T|t)eam", "$1iem"},
      new []{ "ntures", "ntjâhs"}, // 'ventures'
      new []{ "ultur", "ulter"}, // 'culturele'
      new []{ "(?<![Ss])tu(?![âfkurst])", "te"}, // 'culturele', geen 'tua', 'vintage', 'instituut', 'tussenletter', 'stuks', 'aansturing', 'vacatures'
      new []{ "\\bvan je\\b", "vajje"},
      new []{ "\\bvan (het|ut)\\b", "vannut"},
      new []{ "\\b(V|v)ege(?![lz])", "$1eige"}, // 'vegetarisch', geen 'vergelijking', 'vergezeld' (moet voor 'ver -> ve')
      new []{ "vert\\b", "vâht"}, // 'levert'
      new []{ "([Vv])er(?![aeèfrious])", "$1e"}, // wel 'verkoop', geen 'verse', 'veranderd', 'overeenkomsten', 'overige', 'verf/verref', uitgeverij/è
      new []{ "([Vv])er(sl|sta)", "$1e$2"}, // 'verslag', 'verstappen'
      new []{ "vaka", "veka" }, // 'vakantie' 
      new []{ "vard", "vâh"}, // 'boulevard'
      new []{ "voetbal", "foebal"},
      new []{ "we er ", "we d'r "}, // 'we er'
      new []{ "\\ber\\b", "d'r"},
      new []{ "\\bEr\\b", "D'r"},
      new []{ "(I|i)n je\\b", "$1jje"},
      new []{ "wil hem\\b", "wil 'm"},
      new []{ "(W|w|H|h)ee(t|l)", "$1ei$2"}, // 'heel', 'heet'
      new []{ "Young", "Jong" }, // 'Young'
      new []{ "young", "jong" }, // 'young'
      new []{ "yo", "jau" }, // 'yoga'
      new []{ "Yo", "Yau" }, // 'yoga'
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
      new []{ "\\b(D|d)em", "$1eim" }, // 'demissionair'
      new []{ "ssio", "ssiau" }, // 'demissionair'
      new []{ "sol(?![l])", "saul" }, // 'resoluut', geen 'solliciteren'
      new []{ "aine", "ène" }, // 'quarantaine'
      new []{ "tain", "tein" }, // 'vuilcontainer'
      new []{ "terna", "tena"},
      new []{ "(?<![eèuoin])gel\\b", "sjel"}, // 'handgel', geen 'regel', 'pingel', 'vogel'
      new []{ "ingel\\b", "ingol"}, // 'pingel'
      new []{ "ign", "inj"}, // 'rustsignaal'
      new []{ "down", "dâhn" }, // 'lockdown'
      new []{ "lock", "lok"}, // 'lockdown'
      new []{ "(?<![s])sis(?![i])", "sus"}, // 'crisis', geen 'rassis'van 'racist', 'positie'
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
