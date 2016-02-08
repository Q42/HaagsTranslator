﻿using System.Text.RegularExpressions;
using NUnit.Framework;

namespace HaagsTranslator.Tests
{
  [TestFixture]
  public class TranslatorTests
  {
    private Translator translator;

    [SetUp]
    public void SetUp()
    {
      translator = new Translator();
    }
    
    [Test]
    [TestCase("Schilderswijk", "Schildâhswèk")]
    [TestCase("Zuiderpark", "Zùidâhpark")]
    [TestCase("achter. achter, achter ", "achtâh. achtâh, achtâh ")]
    [TestCase("Voorwoord", "Voâhwoâhd")]
    [TestCase("Ik zweer je, meneer, doe je het nog een keer, krijg je een peer, ga je neer.", "Ik zweâh je, meneâh, doejenut nog un keâh, krèg je un peâh, ga je neâh.")]
    [TestCase("directie, aktie.", "direksie, aksie.")]
    [TestCase("Lekker laten hangen. Hier ", "Lekkâh late hangûh. Hieâh ")]
    [TestCase("Vrijheid. Mooie duur Huur er kermis", "Vrèhèd. Mauie duâh Huâh er kerremis")]
    [TestCase("Slimmer naar de pier in Scheveningen Voorburg", "Slimmâh naah de pieâh in Scheiveningâh Voâhburrag")]
    [TestCase("Lekker naar het Binnenhof en daarna gaan stappen.", "Lekkâh naah ut Binnehof en daahna gan stappûh.")]
    [TestCase("Den Haag, het Plein, het Vredespaleis, Zuiderpark, park, Zwarte Pad, Grote Markt, de kerk, Rijswijkseplein", "De Haag, ut Plèn, ut Vreidespelès, Zùidâhparrek, parrek, Zwagte Pad, Graute Marrek, de kerrek, Rèswèkseplèn")]
    [TestCase("Verkoopprijs, prijs Kurhaus, Restaurants, romantisch, romantische, rommel, Maurice Haak Majesteit", "Vekaupprès, près Koeâhhâhs, Eithoeke, raumanties, raumantiese, rommel, Maûpie Majestèt")]
    [TestCase("ADO ADO Den Haag, gefeliciteerd met de verjaardag doosje smarties ", "Adau FC De Haag, gefeiliciteâhd met de vejaahdag daussie smagties ")]
    [TestCase("Gert, barst, Martin, harte smarties, ooievaar dat is raar maar wel waar.", "Gegt, bagst, Magtin, hagte smagties, auievaah da's raah maah wel waah.")]
    [TestCase("Door een kier in de deur van de bar rook ik de meur van goor bier ", "Doâh un kieâh in de deuâh van de bâh rauk ik de meuâh van goâh bieâh ")]
    [TestCase("De juiste uitspraak vanuit Haags wordt dus altijd aangegeven door diverse accenten.", "De jùiste ùitspraak vanùit Haags wogt dus altèd angegeive doâh divegse aksentûh.")]
    [TestCase("Ik houd van jou, goude vrouw voor jou val ik flauw.", "Ik hâh van jâh, gâhwe vrâh voâh jâh vallik flâh.")]
    [TestCase("Die ongein kan wel waar zijn, maar je blijft wel even mooi van mijn lijf, geil wijf", "Die ongèn ken wel waah zèn, maah je blèf wel eive maui van mèn lèf, gèl wèf")]
    [TestCase("Dat is helemaal niet fijn, dat is ronduit. Weet je wat klote is?", "Da's heilemaal nie fèn, da's rondùit. Weit je wat klaute is?")]
    [TestCase("De tram die met opzet heel langzaam rijdt. auto volkslied", "De trem die met opzet heil langzaam rèd. âhtau vollekslied")]
    [TestCase("Ik raak altijd weer tot tranen toe beroerd", "Ik raak altèd weâh tot trane toe beroeâhd")]
    [TestCase("Waarom we ze klinkers noemen.", "Warom we ze klinkâhs noemûh.")]
    [TestCase("Het eerste boekje verscheen tien jaar geleden. Sinds die tijd is er veel veranderd in Den Haag. En vaak ten goede.", "Ut eâhste boekie veschein tien jaah geleije. Sins die tèd istâh veil verandâhd in De Haag. En vaak te goede.")]
    [TestCase("Hoogste tijd dus dat er een herziene en geactualiseerde versie kwam van dit boekje in Haagse kleuren, waarin recht wordt gedaan aan de dynamiek in onze stad.", "Haugste tèd dus dattâh un heâhziene en geaktuwaliseâhde vegsie kwam van dit boekie in Haagse kleure, warin rech wogt gedaan an de dinemiek in onze stad.")]
    [TestCase("Sommige zeggen weleens dat het Haags geen dialect is maar. Immers, dit Haags is van de Hagenezen. Ik meen echter dat er veel overeenkomsten te beluisteren zijn met een andere zogenaamd.", "Sommige zegge welles dat ut Haags gein dialek is maah. Immâhs, dit Haags is van de Hageneize. Ik meint echtâh dattâh veil auvereinkomste te belùistere zèn met un andâh zaugenaamd.")]
    [TestCase("Luister maar eens naar de R zoals veel Hagenaars die uitspreken. Of naar de klanken van de korte klinkers.", "Lùistâh maah 'ns naah de R zauas veil Hagenaahs die ùitspreikûh. Of naah de klanke van de kogte klinkâhs.")]
    [TestCase("Het lijkt allemaal erg veel op elkaar. Haast geen verschil. Het bekakte Haags is wel wat meer geknepen. Of, zoals de samenstellers ergens zeggen omdat ze om die aardappel heen moeten.", "Ut lèk allemaal errag veil op elkaah. Haas gein veschil. Ut bekakte Haags is wel wat meâh gekneipûh. Of, zauas de samestellâhs erreges zegge omdat ze om die aahdappel hein motte.")]
    [TestCase("Eigenlijk zou je dus kunnen zeggen dat alle Hagenaars dezelfde taal spreken, het Haags. Zij het dan met nuanceverschillen.", "Ègelèk zâh je dus kenne zegge dat alle Hagenaahs dezellufde taal spreike, ut Haags. Zè ut dan met nuwanseveschillûh.")]
    [TestCase("Dit standaardwerk is daarom voor de buitenstaander, de niet-Hagenaar of Hagenees, een noodzakelijk instrument om goed Haags te kunnen leren praten. En zich daarmee toegang te verschaffen tot alle Haagse kringen.", "Dit standaahdwerrek is darom voâh de bùitestaandâh, de niet-Hagenaah of Hageneis, un naudzakelèk instrument om goed Haags te kenne lere pratûh. En ze ège daahmei toegang te veschaffe tot alle Haagse kringûh.")]
    [TestCase("Lees het, leer de spelling, blijf de uitspraak oefenen. Knijp indien nodig. En u kan in heel Den Haag terecht. Van de Schilderswijk tot in het Statenkwartier en van de Gouveneurlaan tot op de Dennenweg.", "Leis ut, leâh de spelling, blèf de ùitspraak oefenûh. Knèp indien naudag. En u ken in heil De Haag terech. Van de Schildâhswèk tot innut Statekwagtieâh en van de Goeveneuâhlaan tot op de Denneweg.")]
    [TestCase("Als burgemeester van Den Haag juich ik dit leuke en humoristische initiatief van harte toe. Ik beveel het ten zeerste aan bij iedereen die zich wil verdiepen in de Haagse taal en de Haagse ziel.", "As burragemeistâh van De Haag jùich ik dit leuke en humoristiese initiatief van hagte toe. Ik beveil ut te zeâhste an bè iederein die ze ège wil vediepe in de Haagse taal en de Haagse ziel.")]
    [TestCase("De blote geslachtsdelen schoten hem in het verkeerde keelgat", "De blaute geslachsdeile schaute hem innut vekeâhde keilgat")]
    [TestCase("Iemand dood maken met een blije mus. Ik word met scheve schaatsen aangekeken.", "Iemand daud make met un blèje mus. Ik wogt met scheive schaatse angekeikûh.")]
    [TestCase("Je bent een mongool, een halve zool, een mooie grote klootviool", "Je ben un megaul, un halleve zaul, un mauie graute klautviaul")]
    [TestCase("Je moet geen oude schoenen weggooien voordat je een nieuwe doos hebt.", "Je mot gein âhwe schoene weggauie voâhdat je un nieuwe daus hep.")]
    [TestCase("De wereld is een pijp kaneel, je zuigt er aan met pijn in je keel.", "De wereld issun pèp kaneil, je zùig d'ran met pèn in je keil.")]
    [TestCase("Ik werd urenlang ondergevraagd.", "Ik wegd urelang ondâhgevraag.")]
    [TestCase("Die heeft spullen, die zijn al heel lang oud, zeg maar antiek.", "Die hep spulle, die zèn al heil lang âhd, zeg maah antiek.")]
    [TestCase("koets, prostituee, klokkenluider, tribunaal, onsympatieke, verlaten, politici, bierfeest ", "patsâhbak, prostituwei, klokkelùiâh, tribenaal, onsympetieke, velate, paulitici, bieâhfeis ")]
    [TestCase("natuurlijke barrière, reisbestemming, oude taaie, zo gek als een kaartspel", "natuâhlèke barrèjerre, rèsbestemming, âhwe taaie, zau gek assun kaagtspel")]
    [TestCase("uitnodiging om een boom om te hakken. met z'n alle zo hard als je kan achter een neger aanrennen, City Pier City loop ", "ùitnaudiging om un baum om te hakkûh. met ze alle zau hard asje ken achtâh un negâh anrenne, City Pieâh City laup ")]
    [TestCase("Achter de pui aan het Spui is iedereen in een luie bui ", "Achtâh de pùi annut Spùi is iederein innun lùie bùi ")]
    [TestCase("Geel is heel wat anders als geil, maar je kan er wel allebei een ziekte van krijgen ", "Geil is heil wat andâhs as gèl, maah je ken er wel allebè un ziekte van krège ")]
    [TestCase("Van die hete saté zat ik een uur of twee met m'n reet op de plee aan de diaree ", "Van die heite satei zattik un uâh of twei met me reit op de plei an de diarei ")]// met m'n niet vertaald
    [TestCase("Menstruatie is een bloederige situatie, net als fluctuatie van de ", "Menstruwasie issun bloederige situwasie, net as fluktuwasie van de ")]
    [TestCase("De vla lag in de la van ma en zij lag op d'r pa, die dat niet echt naar of raar vond, medeklinkers ", "De vla lag in de la van ma en zè lag op d'r pa, die dat nie ech naah of raah vond, meideklinkâhs ")]
    [TestCase("Ruud zei resoluut ik huur acuut een wijf a la minuut ", "Ruud zè reisoluut ik huâh acuut un wèf a la minuut ")]
    [TestCase("Is dat je haar of heb je jongen gekregen Maar troost jezelf op een ooievaar groeit helemaal geen haar, dat is raar maar wel waar en zo kennen we er nog wel een paar ", "Is dat je haah of hebbie jonge gekreige Maah traus je ège op un auievaah groeit heilemaal gein haah, da's raah maah wel waah en zau kenne we d'r nog wel un paah ")]
    [TestCase("Die hoer is te duur, dan worden m'n druiven maar zuur ", "Die hoeâh is te duâh, dan wogde me drùive maah zuâh ")]
    [TestCase("Mijn hart smacht met alle macht naar een neut in de nacht. Een racist krijgt een vuist voor z'n puist en hij denkt het wordt nou wel erg zwart ", "Mèn hagt smach met alle mach naah un neut in de nach. Un rassis krèg un vùis voâh ze pùis en hè denk ut wogt nâh wel errag zwagt ")]
    [TestCase("M'n ijs smelt fout gespelt ", "Me ès smelt fâht gespelt ")]
    [TestCase("Het arme volk in Voorburg bad in de kerk heb je geen werk ", "Ut arreme vollek in Voâhburrag bad in de kerrek hebbie gein werrek ")]
    [TestCase("De vlerk stak zijn dolk in de snurkende Turk. elk valk erg", "De vlerrek stak zèn dollek in de snurrekende Turrek. ellek valluk errag")]
    [TestCase("De keeper wil hem nog houden, maar de voetbal hangt al in de touwen. hangen komen komt nemen ", "De keepâh wil 'm nog hâhwe, maah de foebal hank al in de tâhwûh. hange kaume komp neime ")]
    [TestCase("Ik heb een houten bek, heb je een fles olie? Heb je een step om te surfen op het wereldwijde web ", "Ik hep'n hâhte bek, hebbie un fles aulie? Hebbie un steppie om te surrefe op ut wereldwède wep ")]
    [TestCase("Kinderen zijn lastpakken, ik koop nooit geen postzegels van die pusbakken.", "Kindere zèn laspakke, ik kaup nauit gein poszeigels van die pusbakkûh.")]
    [TestCase("bezoeken festivalstad achtergelaten doen hebben zondag avondje bieden uitzicht indeling.", "bezoeke festivalstad achtâhgelate doen hebbe zondag avondje biede ùitzich indeiling.")]
    [TestCase("De godsdienstige stadswacht stond bij de gebedsdienst als een scheidsrechter te huilen ", "De gosdienstige staswach stond bè de gebesdiens assun schèsrechtâh te hùile ")]
    [TestCase("Het wordt almaar kouder, zei de strandpaviljoenhouder. De vermoeide ouder kreeg een schuit in z'n schouder ", "Ut wogt almaah kâhwâh, zè de strandpaviljoenhâhwâh. De vemoeide âhwâh kreig un schùit in ze schâhwâh ")]
    [TestCase("Kan het wat zachter, zei de strandwachter tegen de kreunende verkrachter ", "Ken ut wat zachtâh, zè de strandwachtâh teige de kreunende vekrachtâh ")]
    [TestCase("Als ik een asbak was, vrat ik as als gras", "As ik un asbak was, vrattik as as gras")]
    [TestCase("Krijg jij lekker de kanker", "Krèg jè lekkâh de kankâh")]
    [TestCase("Wilders is groot, Wilders is machtig, hij is een lul van één meter tachtig ", "Wildâhs is graut, Wildâhs is machtag, hè issun lul van ein meitâh tachtag ")]
    [TestCase("De directie van Milieudefensie werd op vakantie door de politie betrapt op incontinentie.", "De direksie van Mejuidefensie wegd op vekansie doâh de paulisie betrap op inkontinensie.")]
    [TestCase("De redactie van het boekje neemt geen notie van de klachten van lezers over inconsequenties ", "De redaksie vannut boekie neimp gein nausie van de klachte van leizâhs auvâh inkonsekwensies ")]
    [TestCase("Als je gaat lopen zeiken zal ik de oren van je harses trekken.", "Asje gaat laupe zèke zallik de ore vajje hagses trekkûh.")]
    [TestCase("Zal ik even het behang van je knars aftrekken.", "Zallik eive ut behang vajje knags aftrekkûh.")]
    [TestCase("Van zo'n bakje koffie krijg ik huisje boompje beestje in m'n koppie", "Van zaun bakkie koffie krèg ik hùissie baumpie beissie imme koppie")]
    public void Given_Dutch_Translates_To_Haags(string dutch, string expectedTranslation)
    {
      var result = translator.Translate(dutch);
      Assert.AreEqual(expectedTranslation, result);
    }
  }
}