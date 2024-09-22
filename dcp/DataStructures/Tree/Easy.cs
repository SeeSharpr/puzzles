using System.Text;
using Xunit.Abstractions;

namespace dcp.DataStructures.Tree
{
    public class Easy
    {
        // Problem #0003
        // Given the root to a binary tree, implement serialize(root), which serializes the tree into a string, and deserialize(s), which deserializes the string back into the tree.
        // For example, given the following Node class
        // class Node :
        //    def __init__(self, val, left=None, right=None):
        //        self.val = val
        //        self.left = left
        //        self.right = right
        // The following test should pass:
        // node = Node('root', Node('left', Node('left.left')), Node('right'))
        // assert deserialize(serialize(node)).left.left.val == 'left.left'    

        private sealed class SBTN(string val, SBTN? left = null, SBTN? right = null)
        {
            public readonly string val = val;
            public readonly SBTN? left = left;
            public readonly SBTN? right = right;
        }

        [Fact]
        public void SerializeBinaryTree()
        {
            static string? Serialize(SBTN? node)
            {
                return node == null
                    ? "()"
                    : string.Concat(
                        "(",
                        Serialize(node.left),
                        Serialize(node.right),
                        node.val,
                        ")");
            }

            static SBTN? Deserialize(ref int index, string? input)
            {
                // Trivial case
                if (input == null) return null;
                
                // Format error
                if (input.Length - index < 2 || input[index] != '(') throw new ArgumentException(input);

                // Null node is ()
                if (input[++index] == ')') { ++index; return null; }

                // Recurr
                SBTN? left = Deserialize(ref index, input);
                SBTN? right = Deserialize(ref index, input);

                // Non-null node has val up to first (
                int valLeft = index;
                int valRight = input.IndexOf(')', valLeft);

                // Format error, again
                if (valRight == -1) throw new ArgumentException(input);
                string val = input.Substring(valLeft, valRight - valLeft);
                index = valRight+1;

                return new SBTN(val, left, right);
            }

            Tuple<SBTN, Func<SBTN?, string?>, string?>[] testCases =
            [
                new Tuple<SBTN, Func<SBTN?, string?>, string?>(new SBTN("root", new SBTN("left", new SBTN("left.left")), new SBTN("right")), node => node?.left?.left?.val, "left.left"),
            ];

            foreach (var tuple in testCases)
            {
                int index = 0;
                Assert.Equal(tuple.Item3, tuple.Item2.Invoke(Deserialize(ref index, Serialize(tuple.Item1))));
            }
        }
    }
}
