using leetcode.Lists.Top150;
using Microsoft.VisualBasic;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using static leetcode.Lists.Top150.GraphBFS;
using static leetcode.Lists.Trie.Medium;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Text;
using System.Collections.Immutable;

namespace leetcode.Lists
{
    public class Trie
    {
        [Trait("Difficulty", "Medium")]
        public class Medium
        {
            public class Trie
            {
                private bool isTerminal = false;
                private readonly Dictionary<char, Trie> children = [];

                public void Insert(string word)
                {
                    Trie? it = this;
                    foreach (var c in word)
                    {
                        if (!it.children.TryGetValue(c, out var child)) it.children.Add(c, child = new());
                        it = child;
                    }

                    it.isTerminal = true;
                }

                public bool Search(string word)
                {
                    Trie? it = this;
                    foreach (var c in word)
                    {
                        if (!it.children.TryGetValue(c, out var child)) return false;
                        it = child;
                    }

                    return it.isTerminal;
                }

                public bool StartsWith(string prefix)
                {
                    Trie? it = this;
                    foreach (var c in prefix)
                    {
                        if (!it.children.TryGetValue(c, out var child)) return false;
                        it = child;
                    }

                    return true;
                }

                public override string ToString()
                {
                    return string.Join("", children.Select(pair => $"{pair.Key}{(pair.Value.isTerminal ? '.' : '_')}"));
                }
            }

            /// <summary>
            /// 208. Implement Trie (Prefix Tree)
            /// A trie(pronounced as "try") or prefix tree is a tree data structure used to efficiently store and retrieve keys in a dataset of strings.There are various applications of this data structure, such as autocomplete and spellchecker.
            /// Implement the Trie class:
            /// Trie() Initializes the trie object.
            /// void insert(String word) Inserts the string word into the trie.
            /// boolean search(String word) Returns true if the string word is in the trie(i.e., was inserted before), and false otherwise.
            /// boolean startsWith(String prefix) Returns true if there is a previously inserted string word that has the prefix prefix, and false otherwise.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/implement-trie-prefix-tree/description/?envType=study-plan-v2&envId=top-interview-150">
            [Theory]
            [InlineData("[Trie,insert,search,search,startsWith,insert,search]", "[[],[apple],[apple],[app],[app],[app],[app]]", "[null,null,true,false,true,null,true]")]
            public void TrieTest(string inputAction, string inputArgs, string output)
            {
                string[] actions = inputAction.Parse1DArray().ToArray();
                string[][] args = inputArgs.Parse2DArray().Select(e => e.ToArray()).ToArray();
                string[] expected = output.Parse1DArray().ToArray();

                Assert.Equal(expected.Length, actions.Length);
                Assert.Equal(expected.Length, args.Length);

                Trie? trie = null;
                for (int i = 0; i < expected.Length; i++)
                {
                    switch (actions[i])
                    {
                        case "Trie":
                            trie = new Trie();
                            break;
                        case "insert":
                            trie!.Insert(args[i][0]);
                            break;
                        case "search":
                            bool expSearch = bool.Parse(expected[i]);
                            bool actSearch = trie!.Search(args[i][0]);
                            Assert.Equal(expSearch, actSearch);
                            break;
                        case "startsWith":
                            bool expStartsWith = bool.Parse(expected[i]);
                            bool actStartsWith = trie!.StartsWith(args[i][0]);
                            break;
                        default:
                            throw new NotImplementedException(actions[i]);
                    }
                }
            }

            public class WordDictionary
            {
                private bool isTerminal = false;
                private readonly Dictionary<char, WordDictionary> children = [];

                public WordDictionary()
                {
                }

                public void AddWord(string word)
                {
                    WordDictionary? it = this;
                    foreach (var c in word)
                    {
                        if (!it.children.TryGetValue(c, out var child)) it.children.Add(c, child = new());
                        it = child;
                    }

                    it.isTerminal = true;
                }

                public bool Search(string word)
                {
                    WordDictionary? it = this;
                    for (int i = 0; i < word.Length; i++)
                    {
                        char c = word[i];

                        if (c == '.')
                        {
                            string rest = word.Substring(i + 1);

                            return it.children.Any(child => child.Value.Search(rest));
                        }
                        else
                        {
                            if (!it.children.TryGetValue(c, out var child)) return false;
                            it = child;
                        }
                    }

                    return it.isTerminal;
                }
            }

            /// <summary>
            /// 211. Design Add and Search Words Data Structure
            /// Design a data structure that supports adding new words and finding if a string matches any previously added string.
            /// Implement the WordDictionary class:
            /// WordDictionary() Initializes the object.
            /// void addWord(word) Adds word to the data structure, it can be matched later.
            /// bool search(word) Returns true if there is any string in the data structure that matches word or false otherwise.word may contain dots '.' where dots can be matched with any letter.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/design-add-and-search-words-data-structure/?envType=study-plan-v2&envId=top-interview-150"/>
            [Theory]
            [InlineData("[WordDictionary,addWord,addWord,addWord,search,search,search,search]", "[[],[bad],[dad],[mad],[pad],[bad],[.ad],[b..]]", "[null,null,null,null,false,true,true,true]")]
            public void WordDictionaryTest(string inputAction, string inputArgs, string output)
            {
                string[] actions = inputAction.Parse1DArray().ToArray();
                string[][] args = inputArgs.Parse2DArray().Select(x => x.ToArray()).ToArray();
                string[] expects = output.Parse1DArray().ToArray();

                Assert.Equal(expects.Length, actions.Length);
                Assert.Equal(expects.Length, args.Length);

                WordDictionary? wd = null;
                for (int i = 0; i < expects.Length; i++)
                {
                    switch (actions[i])
                    {
                        case "WordDictionary":
                            wd = new WordDictionary();
                            break;
                        case "addWord":
                            wd!.AddWord(args[i][0]);
                            break;
                        case "search":
                            Assert.Equal(bool.Parse(expects[i]), wd!.Search(args[i][0]));
                            break;
                        default:
                            throw new NotSupportedException(actions[i]);
                    }
                }
            }
        }

        [Trait("Difficulty", "Hard")]
        public class Hard
        {
            public class Trie
            {
                private bool isTerminal = false;
                private readonly Dictionary<char, Trie> children = [];

                public void Insert(string word)
                {
                    Trie? it = this;
                    foreach (var c in word)
                    {
                        if (!it.children.TryGetValue(c, out var child)) it.children.Add(c, child = new());
                        it = child;
                    }

                    it.isTerminal = true;
                }

                public void Traverse(char[][] matrix, int y, int x, StringBuilder sb, HashSet<string> found)
                {
                    // Stop searching when we reach the limit of the trie
                    if (!children.TryGetValue(matrix[y][x], out var child)) return;

                    // Candidate word
                    char ch = matrix[y][x];
                    sb.Append(ch);
                    matrix[y][x] = '.';

                    if (child.isTerminal) found.Add(sb.ToString());

                    if (y > 0 && matrix[y - 1][x] != '.') child.Traverse(matrix, y - 1, x, sb, found);
                    if (y + 1 < matrix.Length && matrix[y + 1][x] != '.') child.Traverse(matrix, y + 1, x, sb, found);
                    if (x > 0 && matrix[y][x - 1] != '.') child.Traverse(matrix, y, x - 1, sb, found);
                    if (x + 1 < matrix[y].Length && matrix[y][x + 1] != '.') child.Traverse(matrix, y, x + 1, sb, found);

                    // Revert change
                    matrix[y][x] = ch;
                    sb.Remove(sb.Length - 1, 1);
                }

                public override string ToString()
                {
                    return string.Join("", children.Select(pair => $"{pair.Key}{(pair.Value.isTerminal ? '.' : '_')}"));
                }
            }

            /// <summary>
            /// 212. Word Search II
            /// Given an m x n board of characters and a list of strings words, return all words on the board.
            /// Each word must be constructed from letters of sequentially adjacent cells, where adjacent cells are horizontally or vertically neighboring.The same letter cell may not be used more than once in a word.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/word-search-ii/description/?envType=study-plan-v2&envId=top-interview-150"/>
            [Theory]
            [InlineData("[[o,a,a,n],[e,t,a,e],[i,h,k,r],[i,f,l,v]]", "[oath,pea,eat,rain]", "[eat,oath]")]
            [InlineData("[[a,b],[c,d]]", "[abcb]", "[]")]
            [InlineData("[[a,a]]", "[aaa]", "[]")]
            [InlineData("[[a,a,a,a,a,a,a,a,a,a,a,a],[a,a,a,a,a,a,a,a,a,a,a,a],[a,a,a,a,a,a,a,a,a,a,a,a],[a,a,a,a,a,a,a,a,a,a,a,a],[a,a,a,a,a,a,a,a,a,a,a,a],[a,a,a,a,a,a,a,a,a,a,a,a],[a,a,a,a,a,a,a,a,a,a,a,a],[a,a,a,a,a,a,a,a,a,a,a,a],[a,a,a,a,a,a,a,a,a,a,a,a],[a,a,a,a,a,a,a,a,a,a,a,a],[a,a,a,a,a,a,a,a,a,a,a,a],[a,a,a,a,a,a,a,a,a,a,a,a]]", "[lllllll,fffffff,ssss,s,rr,xxxx,ttt,eee,ppppppp,iiiiiiiii,xxxxxxxxxx,pppppp,xxxxxx,yy,jj,ccc,zzz,ffffffff,r,mmmmmmmmm,tttttttt,mm,ttttt,qqqqqqqqqq,z,aaaaaaaa,nnnnnnnnn,v,g,ddddddd,eeeeeeeee,aaaaaaa,ee,n,kkkkkkkkk,ff,qq,vvvvv,kkkk,e,nnn,ooo,kkkkk,o,ooooooo,jjj,lll,ssssssss,mmmm,qqqqq,gggggg,rrrrrrrrrr,iiii,bbbbbbbbb,aaaaaa,hhhh,qqq,zzzzzzzzz,xxxxxxxxx,ww,iiiiiii,pp,vvvvvvvvvv,eeeee,nnnnnnn,nnnnnn,nn,nnnnnnnn,wwwwwwww,vvvvvvvv,fffffffff,aaa,p,ddd,ppppppppp,fffff,aaaaaaaaa,oooooooo,jjjj,xxx,zz,hhhhh,uuuuu,f,ddddddddd,zzzzzz,cccccc,kkkkkk,bbbbbbbb,hhhhhhhhhh,uuuuuuu,cccccccccc,jjjjj,gg,ppp,ccccccccc,rrrrrr,c,cccccccc,yyyyy,uuuu,jjjjjjjj,bb,hhh,l,u,yyyyyy,vvv,mmm,ffffff,eeeeeee,qqqqqqq,zzzzzzzzzz,ggg,zzzzzzz,dddddddddd,jjjjjjj,bbbbb,ttttttt,dddddddd,wwwwwww,vvvvvv,iii,ttttttttt,ggggggg,xx,oooooo,cc,rrrr,qqqq,sssssss,oooo,lllllllll,ii,tttttttttt,uuuuuu,kkkkkkkk,wwwwwwwwww,pppppppppp,uuuuuuuu,yyyyyyy,cccc,ggggg,ddddd,llllllllll,tttt,pppppppp,rrrrrrr,nnnn,x,yyy,iiiiiiiiii,iiiiii,llll,nnnnnnnnnn,aaaaaaaaaa,eeeeeeeeee,m,uuu,rrrrrrrr,h,b,vvvvvvv,ll,vv,mmmmmmm,zzzzz,uu,ccccccc,xxxxxxx,ss,eeeeeeee,llllllll,eeee,y,ppppp,qqqqqq,mmmmmm,gggg,yyyyyyyyy,jjjjjj,rrrrr,a,bbbb,ssssss,sss,ooooo,ffffffffff,kkk,xxxxxxxx,wwwwwwwww,w,iiiiiiii,ffff,dddddd,bbbbbb,uuuuuuuuu,kkkkkkk,gggggggggg,qqqqqqqq,vvvvvvvvv,bbbbbbbbbb,nnnnn,tt,wwww,iiiii,hhhhhhh,zzzzzzzz,ssssssssss,j,fff,bbbbbbb,aaaa,mmmmmmmmmm,jjjjjjjjjj,sssss,yyyyyyyy,hh,q,rrrrrrrrr,mmmmmmmm,wwwww,www,rrr,lllll,uuuuuuuuuu,oo,jjjjjjjjj,dddd,pppp,hhhhhhhhh,kk,gggggggg,xxxxx,vvvv,d,qqqqqqqqq,dd,ggggggggg,t,yyyy,bbb,yyyyyyyyyy,tttttt,ccccc,aa,eeeeee,llllll,kkkkkkkkkk,sssssssss,i,hhhhhh,oooooooooo,wwwwww,ooooooooo,zzzz,k,hhhhhhhh,aaaaa,mmmmm]", "[a,aa,aaa,aaaa,aaaaa,aaaaaa,aaaaaaa,aaaaaaaa,aaaaaaaaa,aaaaaaaaaa]")]
            [InlineData("[[m,b,c,d,e,f,g,h,i,j,k,l],[n,a,a,a,a,a,a,a,a,a,a,a],[o,a,a,a,a,a,a,a,a,a,a,a],[p,a,a,a,a,a,a,a,a,a,a,a],[q,a,a,a,a,a,a,a,a,a,a,a],[r,a,a,a,a,a,a,a,a,a,a,a],[s,a,a,a,a,a,a,a,a,a,a,a],[t,a,a,a,a,a,a,a,a,a,a,a],[u,a,a,a,a,a,a,a,a,a,a,a],[v,a,a,a,a,a,a,a,a,a,a,a],[w,a,a,a,a,a,a,a,a,a,a,a],[x,y,z,a,a,a,a,a,a,a,a,a]]", "[aaaaaaaaaa,aaaaaaaaab,aaaaaaaaac,aaaaaaaaad,aaaaaaaaae,aaaaaaaaaf,aaaaaaaaag,aaaaaaaaah,aaaaaaaaai,aaaaaaaaaj,aaaaaaaaak,aaaaaaaaal,aaaaaaaaam,aaaaaaaaan,aaaaaaaaao,aaaaaaaaap,aaaaaaaaaq,aaaaaaaaar,aaaaaaaaas,aaaaaaaaat,aaaaaaaaau,aaaaaaaaav,aaaaaaaaaw,aaaaaaaaax,aaaaaaaaay,aaaaaaaaaz,aaaaaaaaba,aaaaaaaabb,aaaaaaaabc,aaaaaaaabd,aaaaaaaabe,aaaaaaaabf,aaaaaaaabg,aaaaaaaabh,aaaaaaaabi,aaaaaaaabj,aaaaaaaabk,aaaaaaaabl,aaaaaaaabm,aaaaaaaabn,aaaaaaaabo,aaaaaaaabp,aaaaaaaabq,aaaaaaaabr,aaaaaaaabs,aaaaaaaabt,aaaaaaaabu,aaaaaaaabv,aaaaaaaabw,aaaaaaaabx,aaaaaaaaby,aaaaaaaabz,aaaaaaaaca,aaaaaaaacb,aaaaaaaacc,aaaaaaaacd,aaaaaaaace,aaaaaaaacf,aaaaaaaacg,aaaaaaaach,aaaaaaaaci,aaaaaaaacj,aaaaaaaack,aaaaaaaacl,aaaaaaaacm,aaaaaaaacn,aaaaaaaaco,aaaaaaaacp,aaaaaaaacq,aaaaaaaacr,aaaaaaaacs,aaaaaaaact,aaaaaaaacu,aaaaaaaacv,aaaaaaaacw,aaaaaaaacx,aaaaaaaacy,aaaaaaaacz,aaaaaaaada,aaaaaaaadb,aaaaaaaadc,aaaaaaaadd,aaaaaaaade,aaaaaaaadf,aaaaaaaadg,aaaaaaaadh,aaaaaaaadi,aaaaaaaadj,aaaaaaaadk,aaaaaaaadl,aaaaaaaadm,aaaaaaaadn,aaaaaaaado,aaaaaaaadp,aaaaaaaadq,aaaaaaaadr,aaaaaaaads,aaaaaaaadt,aaaaaaaadu,aaaaaaaadv,aaaaaaaadw,aaaaaaaadx,aaaaaaaady,aaaaaaaadz,aaaaaaaaea,aaaaaaaaeb,aaaaaaaaec,aaaaaaaaed,aaaaaaaaee,aaaaaaaaef,aaaaaaaaeg,aaaaaaaaeh,aaaaaaaaei,aaaaaaaaej,aaaaaaaaek,aaaaaaaael,aaaaaaaaem,aaaaaaaaen,aaaaaaaaeo,aaaaaaaaep,aaaaaaaaeq,aaaaaaaaer,aaaaaaaaes,aaaaaaaaet,aaaaaaaaeu,aaaaaaaaev,aaaaaaaaew,aaaaaaaaex,aaaaaaaaey,aaaaaaaaez,aaaaaaaafa,aaaaaaaafb,aaaaaaaafc,aaaaaaaafd,aaaaaaaafe,aaaaaaaaff,aaaaaaaafg,aaaaaaaafh,aaaaaaaafi,aaaaaaaafj,aaaaaaaafk,aaaaaaaafl,aaaaaaaafm,aaaaaaaafn,aaaaaaaafo,aaaaaaaafp,aaaaaaaafq,aaaaaaaafr,aaaaaaaafs,aaaaaaaaft,aaaaaaaafu,aaaaaaaafv,aaaaaaaafw,aaaaaaaafx,aaaaaaaafy,aaaaaaaafz,aaaaaaaaga,aaaaaaaagb,aaaaaaaagc,aaaaaaaagd,aaaaaaaage,aaaaaaaagf,aaaaaaaagg,aaaaaaaagh,aaaaaaaagi,aaaaaaaagj,aaaaaaaagk,aaaaaaaagl,aaaaaaaagm,aaaaaaaagn,aaaaaaaago,aaaaaaaagp,aaaaaaaagq,aaaaaaaagr,aaaaaaaags,aaaaaaaagt,aaaaaaaagu,aaaaaaaagv,aaaaaaaagw,aaaaaaaagx,aaaaaaaagy,aaaaaaaagz,aaaaaaaaha,aaaaaaaahb,aaaaaaaahc,aaaaaaaahd,aaaaaaaahe,aaaaaaaahf,aaaaaaaahg,aaaaaaaahh,aaaaaaaahi,aaaaaaaahj,aaaaaaaahk,aaaaaaaahl,aaaaaaaahm,aaaaaaaahn,aaaaaaaaho,aaaaaaaahp,aaaaaaaahq,aaaaaaaahr,aaaaaaaahs,aaaaaaaaht,aaaaaaaahu,aaaaaaaahv,aaaaaaaahw,aaaaaaaahx,aaaaaaaahy,aaaaaaaahz,aaaaaaaaia,aaaaaaaaib,aaaaaaaaic,aaaaaaaaid,aaaaaaaaie,aaaaaaaaif,aaaaaaaaig,aaaaaaaaih,aaaaaaaaii,aaaaaaaaij,aaaaaaaaik,aaaaaaaail,aaaaaaaaim,aaaaaaaain,aaaaaaaaio,aaaaaaaaip,aaaaaaaaiq,aaaaaaaair,aaaaaaaais,aaaaaaaait,aaaaaaaaiu,aaaaaaaaiv,aaaaaaaaiw,aaaaaaaaix,aaaaaaaaiy,aaaaaaaaiz,aaaaaaaaja,aaaaaaaajb,aaaaaaaajc,aaaaaaaajd,aaaaaaaaje,aaaaaaaajf,aaaaaaaajg,aaaaaaaajh,aaaaaaaaji,aaaaaaaajj,aaaaaaaajk,aaaaaaaajl,aaaaaaaajm,aaaaaaaajn,aaaaaaaajo,aaaaaaaajp,aaaaaaaajq,aaaaaaaajr,aaaaaaaajs,aaaaaaaajt,aaaaaaaaju,aaaaaaaajv,aaaaaaaajw,aaaaaaaajx,aaaaaaaajy,aaaaaaaajz,aaaaaaaaka,aaaaaaaakb,aaaaaaaakc,aaaaaaaakd,aaaaaaaake,aaaaaaaakf,aaaaaaaakg,aaaaaaaakh,aaaaaaaaki,aaaaaaaakj,aaaaaaaakk,aaaaaaaakl,aaaaaaaakm,aaaaaaaakn,aaaaaaaako,aaaaaaaakp,aaaaaaaakq,aaaaaaaakr,aaaaaaaaks,aaaaaaaakt,aaaaaaaaku,aaaaaaaakv,aaaaaaaakw,aaaaaaaakx,aaaaaaaaky,aaaaaaaakz,aaaaaaaala,aaaaaaaalb,aaaaaaaalc,aaaaaaaald,aaaaaaaale,aaaaaaaalf,aaaaaaaalg,aaaaaaaalh,aaaaaaaali,aaaaaaaalj,aaaaaaaalk,aaaaaaaall,aaaaaaaalm,aaaaaaaaln,aaaaaaaalo,aaaaaaaalp,aaaaaaaalq,aaaaaaaalr,aaaaaaaals,aaaaaaaalt,aaaaaaaalu,aaaaaaaalv,aaaaaaaalw,aaaaaaaalx,aaaaaaaaly,aaaaaaaalz,aaaaaaaama,aaaaaaaamb,aaaaaaaamc,aaaaaaaamd,aaaaaaaame,aaaaaaaamf,aaaaaaaamg,aaaaaaaamh,aaaaaaaami,aaaaaaaamj,aaaaaaaamk,aaaaaaaaml,aaaaaaaamm,aaaaaaaamn,aaaaaaaamo,aaaaaaaamp,aaaaaaaamq,aaaaaaaamr,aaaaaaaams,aaaaaaaamt,aaaaaaaamu,aaaaaaaamv,aaaaaaaamw,aaaaaaaamx,aaaaaaaamy,aaaaaaaamz,aaaaaaaana,aaaaaaaanb,aaaaaaaanc,aaaaaaaand,aaaaaaaane,aaaaaaaanf,aaaaaaaang,aaaaaaaanh,aaaaaaaani,aaaaaaaanj,aaaaaaaank,aaaaaaaanl,aaaaaaaanm,aaaaaaaann,aaaaaaaano,aaaaaaaanp,aaaaaaaanq,aaaaaaaanr,aaaaaaaans,aaaaaaaant,aaaaaaaanu,aaaaaaaanv,aaaaaaaanw,aaaaaaaanx,aaaaaaaany,aaaaaaaanz,aaaaaaaaoa,aaaaaaaaob,aaaaaaaaoc,aaaaaaaaod,aaaaaaaaoe,aaaaaaaaof,aaaaaaaaog,aaaaaaaaoh,aaaaaaaaoi,aaaaaaaaoj,aaaaaaaaok,aaaaaaaaol,aaaaaaaaom,aaaaaaaaon,aaaaaaaaoo,aaaaaaaaop,aaaaaaaaoq,aaaaaaaaor,aaaaaaaaos,aaaaaaaaot,aaaaaaaaou,aaaaaaaaov,aaaaaaaaow,aaaaaaaaox,aaaaaaaaoy,aaaaaaaaoz,aaaaaaaapa,aaaaaaaapb,aaaaaaaapc,aaaaaaaapd,aaaaaaaape,aaaaaaaapf,aaaaaaaapg,aaaaaaaaph,aaaaaaaapi,aaaaaaaapj,aaaaaaaapk,aaaaaaaapl,aaaaaaaapm,aaaaaaaapn,aaaaaaaapo,aaaaaaaapp,aaaaaaaapq,aaaaaaaapr,aaaaaaaaps,aaaaaaaapt,aaaaaaaapu,aaaaaaaapv,aaaaaaaapw,aaaaaaaapx,aaaaaaaapy,aaaaaaaapz,aaaaaaaaqa,aaaaaaaaqb,aaaaaaaaqc,aaaaaaaaqd,aaaaaaaaqe,aaaaaaaaqf,aaaaaaaaqg,aaaaaaaaqh,aaaaaaaaqi,aaaaaaaaqj,aaaaaaaaqk,aaaaaaaaql,aaaaaaaaqm,aaaaaaaaqn,aaaaaaaaqo,aaaaaaaaqp,aaaaaaaaqq,aaaaaaaaqr,aaaaaaaaqs,aaaaaaaaqt,aaaaaaaaqu,aaaaaaaaqv,aaaaaaaaqw,aaaaaaaaqx,aaaaaaaaqy,aaaaaaaaqz,aaaaaaaara,aaaaaaaarb,aaaaaaaarc,aaaaaaaard,aaaaaaaare,aaaaaaaarf,aaaaaaaarg,aaaaaaaarh,aaaaaaaari,aaaaaaaarj,aaaaaaaark,aaaaaaaarl,aaaaaaaarm,aaaaaaaarn,aaaaaaaaro,aaaaaaaarp,aaaaaaaarq,aaaaaaaarr,aaaaaaaars,aaaaaaaart,aaaaaaaaru,aaaaaaaarv,aaaaaaaarw,aaaaaaaarx,aaaaaaaary,aaaaaaaarz,aaaaaaaasa,aaaaaaaasb,aaaaaaaasc,aaaaaaaasd,aaaaaaaase,aaaaaaaasf,aaaaaaaasg,aaaaaaaash,aaaaaaaasi,aaaaaaaasj,aaaaaaaask,aaaaaaaasl,aaaaaaaasm,aaaaaaaasn,aaaaaaaaso,aaaaaaaasp,aaaaaaaasq,aaaaaaaasr,aaaaaaaass,aaaaaaaast,aaaaaaaasu,aaaaaaaasv,aaaaaaaasw,aaaaaaaasx,aaaaaaaasy,aaaaaaaasz,aaaaaaaata,aaaaaaaatb,aaaaaaaatc,aaaaaaaatd,aaaaaaaate,aaaaaaaatf,aaaaaaaatg,aaaaaaaath,aaaaaaaati,aaaaaaaatj,aaaaaaaatk,aaaaaaaatl,aaaaaaaatm,aaaaaaaatn,aaaaaaaato,aaaaaaaatp,aaaaaaaatq,aaaaaaaatr,aaaaaaaats,aaaaaaaatt,aaaaaaaatu,aaaaaaaatv,aaaaaaaatw,aaaaaaaatx,aaaaaaaaty,aaaaaaaatz,aaaaaaaaua,aaaaaaaaub,aaaaaaaauc,aaaaaaaaud,aaaaaaaaue,aaaaaaaauf,aaaaaaaaug,aaaaaaaauh,aaaaaaaaui,aaaaaaaauj,aaaaaaaauk,aaaaaaaaul,aaaaaaaaum,aaaaaaaaun,aaaaaaaauo,aaaaaaaaup,aaaaaaaauq,aaaaaaaaur,aaaaaaaaus,aaaaaaaaut,aaaaaaaauu,aaaaaaaauv,aaaaaaaauw,aaaaaaaaux,aaaaaaaauy,aaaaaaaauz,aaaaaaaava,aaaaaaaavb,aaaaaaaavc,aaaaaaaavd,aaaaaaaave,aaaaaaaavf,aaaaaaaavg,aaaaaaaavh,aaaaaaaavi,aaaaaaaavj,aaaaaaaavk,aaaaaaaavl,aaaaaaaavm,aaaaaaaavn,aaaaaaaavo,aaaaaaaavp,aaaaaaaavq,aaaaaaaavr,aaaaaaaavs,aaaaaaaavt,aaaaaaaavu,aaaaaaaavv,aaaaaaaavw,aaaaaaaavx,aaaaaaaavy,aaaaaaaavz,aaaaaaaawa,aaaaaaaawb,aaaaaaaawc,aaaaaaaawd,aaaaaaaawe,aaaaaaaawf,aaaaaaaawg,aaaaaaaawh,aaaaaaaawi,aaaaaaaawj,aaaaaaaawk,aaaaaaaawl,aaaaaaaawm,aaaaaaaawn,aaaaaaaawo,aaaaaaaawp,aaaaaaaawq,aaaaaaaawr,aaaaaaaaws,aaaaaaaawt,aaaaaaaawu,aaaaaaaawv,aaaaaaaaww,aaaaaaaawx,aaaaaaaawy,aaaaaaaawz,aaaaaaaaxa,aaaaaaaaxb,aaaaaaaaxc,aaaaaaaaxd,aaaaaaaaxe,aaaaaaaaxf,aaaaaaaaxg,aaaaaaaaxh,aaaaaaaaxi,aaaaaaaaxj,aaaaaaaaxk,aaaaaaaaxl,aaaaaaaaxm,aaaaaaaaxn,aaaaaaaaxo,aaaaaaaaxp,aaaaaaaaxq,aaaaaaaaxr,aaaaaaaaxs,aaaaaaaaxt,aaaaaaaaxu,aaaaaaaaxv,aaaaaaaaxw,aaaaaaaaxx,aaaaaaaaxy,aaaaaaaaxz,aaaaaaaaya,aaaaaaaayb,aaaaaaaayc,aaaaaaaayd,aaaaaaaaye,aaaaaaaayf,aaaaaaaayg,aaaaaaaayh,aaaaaaaayi,aaaaaaaayj,aaaaaaaayk,aaaaaaaayl,aaaaaaaaym,aaaaaaaayn,aaaaaaaayo,aaaaaaaayp,aaaaaaaayq,aaaaaaaayr,aaaaaaaays,aaaaaaaayt,aaaaaaaayu,aaaaaaaayv,aaaaaaaayw,aaaaaaaayx,aaaaaaaayy,aaaaaaaayz,aaaaaaaaza,aaaaaaaazb,aaaaaaaazc,aaaaaaaazd,aaaaaaaaze,aaaaaaaazf,aaaaaaaazg,aaaaaaaazh,aaaaaaaazi,aaaaaaaazj,aaaaaaaazk,aaaaaaaazl,aaaaaaaazm,aaaaaaaazn,aaaaaaaazo,aaaaaaaazp,aaaaaaaazq,aaaaaaaazr,aaaaaaaazs,aaaaaaaazt,aaaaaaaazu,aaaaaaaazv,aaaaaaaazw,aaaaaaaazx,aaaaaaaazy,aaaaaaaazz]", "[aaaaaaaaij,aaaaaaaaih,aaaaaaaaaj,aaaaaaaaaa,aaaaaaaaah,aaaaaaaagh,aaaaaaaagf,aaaaaaaaaf,aaaaaaaaap,aaaaaaaaon,aaaaaaaaop,aaaaaaaaef,aaaaaaaaed,aaaaaaaaar,aaaaaaaaqp,aaaaaaaaqr,aaaaaaaaad,aaaaaaaaat,aaaaaaaasr,aaaaaaaast,aaaaaaaacd,aaaaaaaacb,aaaaaaaaav,aaaaaaaaut,aaaaaaaauv,aaaaaaaajk,aaaaaaaaji,aaaaaaaaak,aaaaaaaaai,aaaaaaaahi,aaaaaaaahg,aaaaaaaaag,aaaaaaaaao,aaaaaaaafg,aaaaaaaafe,aaaaaaaaaq,aaaaaaaapo,aaaaaaaapq,aaaaaaaabc,aaaaaaaabm,aaaaaaaanm,aaaaaaaano,aaaaaaaaae,aaaaaaaaas,aaaaaaaarq,aaaaaaaars,aaaaaaaade,aaaaaaaadc,aaaaaaaaau,aaaaaaaats,aaaaaaaatu,aaaaaaaakl,aaaaaaaakj,aaaaaaaaal,aaaaaaaaab,aaaaaaaaan,aaaaaaaalk,aaaaaaaaac,aaaaaaaaay,aaaaaaaaaw,aaaaaaaavu,aaaaaaaavw,aaaaaaaaaz,aaaaaaaayz,aaaaaaaayx,aaaaaaaawv,aaaaaaaawx,aaaaaaaaza,aaaaaaaazy]")]
            public void FindWords(string inputBoard, string inputWords, string output)
            {
                char[][] board = inputBoard.Parse2DArray(x => x[0]).Select(x => x.ToArray()).ToArray();
                string[] words = inputWords.Parse1DArray().ToArray();
                IList<string> expected = output.Parse1DArray().ToList();

                Trie trie = new();
                foreach (var word in words)
                {
                    trie.Insert(word);
                }

                HashSet<string> seen = [];

                int m = board.Length;
                int n = board[0].Length;
                for (int j = 0; j < m; j++)
                {
                    for (int i = 0; i < n; i++)
                    {
                        trie.Traverse(board, j, i, new(), seen);
                    }
                }

                List<string> actual = seen.ToList();

                Assert.Equal(expected.ToImmutableSortedSet(), actual.ToImmutableSortedSet());
            }
        }
    }
}