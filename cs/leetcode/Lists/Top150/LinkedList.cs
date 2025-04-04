using leetcode.Types.LinkedList;
using System.Collections.Generic;
using System;
using System.Reflection;

namespace leetcode.Lists.Top150
{
    public class LinkedList
    {
        [Trait("Difficulty", "Easy")]
        public class Easy
        {
            /// <summary>
            /// 21. Merge Two Sorted Lists
            /// You are given the heads of two sorted linked lists list1 and list2.
            /// Merge the two lists into one sorted list.The list should be made by splicing together the nodes of the first two lists.
            /// Return the head of the merged linked list.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/merge-two-sorted-lists/description/"/>
            [Trait("List", "TopInterview150")]
            [Trait("Company", "Amazon")]
            [Trait("Company", "Meta")]
            [Theory]
            [InlineData("[1,2,4]", "[1,3,4]", "[1,1,2,3,4,4]")]
            [InlineData("[]", "[]", "[]")]
            [InlineData("[]", "[0]", "[0]")]
            public void MergeTwoLists(string inputL1, string inputL2, string output)
            {
                ListNode? list1 = ListNode.ParseFromLC(inputL1);
                ListNode? list2 = ListNode.ParseFromLC(inputL2);
                ListNode? expected = ListNode.ParseFromLC(output);
                HashSet<int> nodeIds = [];
                for (ListNode? ptr = list1; ptr != null; ptr = ptr.next) nodeIds.Add(ptr.id);
                for (ListNode? ptr = list2; ptr != null; ptr = ptr.next) nodeIds.Add(ptr.id);

                static ListNode? Recursion(ListNode? list1, ListNode? list2)
                {
                    if (list1 == null) return list2;
                    if (list2 == null) return list1;

                    if (list1?.val < list2?.val)
                    {
                        list1.next = Recursion(list1.next, list2);
                        return list1;
                    }
                    else
                    {
                        list2!.next = Recursion(list1, list2.next);
                        return list2;
                    }
                }

                static ListNode? PredNode(ListNode? list1, ListNode? list2)
                {
                    ListNode predHead = new(0);
                    ListNode? predNode = predHead;

                    while (list1 != null && list2 != null)
                    {
                        if (list1?.val < list2?.val)
                        {
                            predNode!.next = list1;
                            list1 = list1?.next;
                        }
                        else
                        {
                            predNode!.next = list2;
                            list2 = list2?.next;
                        }

                        predNode = predNode?.next;
                    }

                    predNode!.next = list1 ?? list2;

                    return predHead.next;
                }

                string solution = "prednode";
                ListNode? actual =
                    solution == "recursion" ? Recursion(list1, list2) :
                    solution == "prednode" ? PredNode(list1, list2) :
                    throw new NotSupportedException(solution);

                for (; actual != null && expected != null; actual = actual.next, expected = expected.next)
                {
                    Assert.Equal(actual.val, expected.val);
                    Assert.True(nodeIds.Remove(actual!.id));
                }

                Assert.Null(actual);
                Assert.Null(expected);
                Assert.Empty(nodeIds);
            }

        }


        [Trait("Difficulty", "Medium")]
        public class Medium
        {
            /// <summary>
            /// 2. Add Two Numbers
            /// You are given two non-empty linked lists representing two non-negative integers. The digits are stored in reverse order, and each of their nodes contains a single digit. Add the two numbers and return the sum as a linked list.
            /// You may assume the two numbers do not contain any leading zero, except the number 0 itself.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/add-two-numbers/description/"/>
            [Trait("List", "TopInterview150")]
            [Trait("Company", "Meta")]
            [Theory]
            [InlineData("[2, 4, 3]", "[5, 6, 4]", "[7, 0, 8]")]
            [InlineData("[0]", "[0]", "[0]")]
            [InlineData("[9, 9, 9, 9, 9, 9, 9]", "[9, 9, 9, 9]", "[8, 9, 9, 9, 0, 0, 0, 1]")]
            public void AddTwoNumbers(string inputL1, string inputL2, string output)
            {
                ListNode? l1 = ListNode.ParseFromLC(inputL1);
                ListNode? l2 = ListNode.ParseFromLC(inputL2);
                ListNode? expected = ListNode.ParseFromLC(output);

                ListNode? result = null;
                ListNode? previous = null;

                int carry = 0;
                while (l1 != null || l2 != null)
                {
                    int sum = (l1?.val ?? 0) + (l2?.val ?? 0) + carry;
                    int digit = sum % 10;
                    carry = sum / 10;

                    if (previous == null)
                    {
                        previous = new ListNode(digit);
                        result = previous;
                    }
                    else
                    {
                        previous.next = new ListNode(digit);
                        previous = previous.next;
                    }

                    l1 = l1?.next;
                    l2 = l2?.next;
                }

                if (carry > 0)
                {
                    if (previous == null)
                    {
                        previous = new ListNode(carry);
                        result = previous;
                    }
                    else
                    {
                        previous.next = new ListNode(carry);
                        previous = previous.next;
                    }
                }

                ListNode? ptr1 = result;
                ListNode? ptr2 = expected;

                while (ptr1 != null && ptr2 != null)
                {
                    Assert.Equal(ptr1.val, ptr2.val);
                    ptr1 = ptr1.next;
                    ptr2 = ptr2.next;
                }

                Assert.Null(ptr1);
                Assert.Null(ptr2);
            }

            /// <summary>
            /// 138. Copy List with Random Pointer
            /// A linked list of length n is given such that each node contains an additional random pointer, which could point to any node in the list, or null.
            /// Construct a deep copy of the list.The deep copy should consist of exactly n brand new nodes, where each new node has its value set to the value of its corresponding original node.Both the next and random pointer of the new nodes should point to new nodes in the copied list such that the pointers in the original list and copied list represent the same list state.None of the pointers in the new list should point to nodes in the original list.
            /// For example, if there are two nodes X and Y in the original list, where X.random --> Y, then for the corresponding two nodes x and y in the copied list, x.random --> y.
            /// Return the head of the copied linked list.
            /// The linked list is represented in the input/output as a list of n nodes.Each node is represented as a pair of[val, random_index] where:
            /// val: an integer representing Node.val
            /// random_index: the index of the node (range from 0 to n-1) that the random pointer points to, or null if it does not point to any node.
            /// Your code will only be given the head of the original linked list.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/copy-list-with-random-pointer/description/"/>
            [Trait("Company", "Amazon")]
            [Trait("Company", "Meta")]
            [Trait("List", "TopInterview150")]
            [Theory]
            [InlineData("[[7,-1],[13,0],[11,4],[10,2],[1,0]]")]
            [InlineData("[[1,1],[2,1]]")]
            [InlineData("[[3,-1],[3,0],[3,-1]]")]
            public void CopyRandomList(string input)
            {
                // -- Test setup
                int[][] data = input.ParseNestedArrayStringLC(int.Parse).Select(s => s.ToArray()).ToArray();
                Node? head = data
                    .Select(d => d[0])
                    .Reverse()
                    .Aggregate(
                        (Node?)null,
                        (next, val) =>
                        {
                            Node? node = new(val);
                            node.next = next;
                            return node;
                        });

                Node? ptr = head;
                foreach (int index in data.Select(d => d[1]))
                {
                    if (index != -1)
                    {
                        Node? random = head;
                        for (int i = 0; i < index; i++)
                        {
                            random = random?.next;
                        }

                        ptr!.random = random;
                    }

                    ptr = ptr?.next;
                }
                // --- Implementation
                static Node? TraverseTwice(Node? head)
                {
                    Dictionary<Node, Node> nodeMap = [];

                    for (Node? oldNode = head; oldNode != null; oldNode = oldNode?.next)
                    {
                        nodeMap.Add(oldNode, new Node(oldNode.val));
                    }

                    for (Node? oldNode = head; oldNode != null; oldNode = oldNode?.next)
                    {
                        Node? newNode = nodeMap[oldNode];
                        newNode.next = oldNode.next == null ? null : nodeMap[oldNode.next];
                        newNode.random = oldNode.random == null ? null : nodeMap[oldNode.random];
                    }

                    return head == null ? null : nodeMap[head];
                }

                static Node? TraverseOnce(Node? head)
                {
                    Dictionary<Node, Node> nodeMap = [];

                    for (Node? oldNode = head; oldNode != null; oldNode = oldNode?.next)
                    {
                        if (!nodeMap.TryGetValue(oldNode, out Node? newNode))
                        {
                            nodeMap[oldNode] = newNode = new Node(oldNode.val);
                        }

                        if (oldNode.next != null)
                        {
                            if (!nodeMap.TryGetValue(oldNode.next, out Node? newNext)) nodeMap[oldNode.next] = newNext = new Node(oldNode.next.val);

                            newNode.next = nodeMap[oldNode.next];
                        }

                        if (oldNode.random != null)
                        {
                            if (!nodeMap.TryGetValue(oldNode.random, out Node? newRandom)) nodeMap[oldNode.random] = newRandom = new Node(oldNode.random.val);

                            newNode.random = nodeMap[oldNode.random];
                        }
                    }

                    return head == null ? null : nodeMap[head];
                }

                static Node? Interleave(Node? head)
                {
                    if (head == null) return head;

                    // Create a weaved copy of the list (original/clone/original/clone...)
                    for (Node? srcNode = head; srcNode != null;)
                    {
                        Node? tgtNode = new(srcNode.val);

                        tgtNode.next = srcNode.next;
                        srcNode.next = tgtNode;
                        srcNode = tgtNode.next;
                    }

                    Node? tgtHead = head.next;

                    // Assign randoms
                    for (Node? srcNode = head, tgtNode = tgtHead; srcNode != null && tgtNode != null; srcNode = srcNode.next?.next, tgtNode = tgtNode.next?.next)
                    {
                        tgtNode.random = srcNode.random?.next;
                    }

                    // Unweave
                    for (Node? srcNode = head, tgtNode = tgtHead; srcNode != null && tgtNode != null; srcNode = srcNode.next, tgtNode = tgtNode.next)
                    {
                        srcNode.next = tgtNode.next;
                        tgtNode.next = srcNode.next?.next;
                    }

                    return tgtHead;
                }

                string solution = nameof(Interleave);
                Node? actual =
                    solution == nameof(TraverseTwice) ? TraverseTwice(head) :
                    solution == nameof(TraverseOnce) ? TraverseOnce(head) :
                    solution == nameof(Interleave) ? Interleave(head) :
                    throw new NotSupportedException(solution);

                // -- Test validation

                SortedSet<int> idsLeft = [];
                SortedSet<int> idsRight = [];
                Dictionary<Node, Node> map = [];
                for (Node? left = head, right = actual; left != null && right != null; left = left.next, right = right.next)
                {
                    map[left] = right;
                    Assert.True(idsLeft.Add(left.id));
                    Assert.True(idsRight.Add(right.id));
                }

                // Id's do not overlap
                Assert.Empty(idsLeft.Intersect(idsRight));

                for (Node? left = head, right = actual; left != null && right != null; left = left.next, right = right.next)
                {
                    // Values match
                    Assert.Equal(right.val, map[left].val);

                    // Id's don't get reused
                    Assert.True(idsLeft.Remove(left.id));
                    Assert.True(idsRight.Remove(right.id));

                    // Random points to the equivalent node
                    Assert.True(right.random != null && left.random != null || right.random == null && left.random == null);
                    if (left.random != null) Assert.Equal(right.random, map[left.random]);
                }
            }

            /// <summary>
            /// 143. Reorder List
            /// You are given the head of a singly linked-list.The list can be represented as:
            /// L0 → L1 → … → Ln - 1 → Ln
            /// Reorder the list to be on the following form:
            /// L0 → Ln → L1 → Ln - 1 → L2 → Ln - 2 → …
            /// You may not modify the values in the list's nodes. Only nodes themselves may be changed.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/reorder-list/description/"/>
            [Trait("Company", "Meta")]
            [Theory]
            [InlineData("[1,2,3,4]", "[1,4,2,3]")]
            [InlineData("[1,2,3,4,5]", "[1,5,2,4,3]")]
            public void ReorderList(string input, string output)
            {
                ListNode? head = ListNode.ParseFromLC(input);
                ListNode? expected = ListNode.ParseFromLC(output);

                int[] inputIds = head?.Select(n => n.id)?.ToArray() ?? [];
                int[] inputValues = input.Parse1DArray(int.Parse).ToArray();
                Dictionary<int, int> inputMap = [];
                for (int i = 0; i < inputIds.Length; i++)
                {
                    inputMap[inputValues[i]] = inputIds[i];
                }

                // ---

                static void UsingStack(ListNode? head)
                {

                    Stack<ListNode> left = new();
                    Stack<ListNode> right = new();

                    int count = 0;
                    for (ListNode? n = head; n != null; n = n.next)
                    {
                        left.Push(n);
                        count++;
                    }

                    count /= 2;
                    for (int i = 0; i < count; i++)
                    {
                        right.Push(left.Pop());
                    }

                    // Now stacks have 1/2/3/(x) and 4/5/6. All we need to do is to take the middle node (if one exists) and
                    // connect the new head and new tail to the tops of the stacks.
                    head = left.Count - right.Count == 0 ? null : left.Pop();

                    if (head != null) head.next = null;

                    while (left.TryPop(out var leftNode) && right.TryPop(out var rightNode))
                    {
                        leftNode.next = rightNode;
                        rightNode.next = head;
                        head = leftNode;
                    }
                }

                static void UsingFastSlow(ListNode? head)
                {
                    if (head == null) return;

                    // Find middle
                    ListNode? middle = head;
                    ListNode? predMiddle = null;
                    for (ListNode? fast = head; fast != null; fast = fast?.next?.next)
                    {
                        predMiddle = middle;
                        middle = middle?.next;
                    }

                    // Cut the middle
                    if (predMiddle != null) predMiddle.next = null;

                    // Invert the second half
                    for (ListNode? curr = middle, pred = null; curr != null;)
                    {
                        ListNode? temp = curr?.next;
                        curr!.next = pred;
                        middle = pred = curr;
                        curr = temp;
                    }

                    // First half is on head, second is on curr. Now merge
                    for (ListNode? left = head, right = middle; left != null && right != null;)
                    {
                        ListNode? leftNext = left.next;
                        ListNode? rightNext = right.next;

                        left.next = right;
                        right.next = leftNext;

                        left = leftNext;
                        right = rightNext;
                    }
                }

                string solution = "fastslow";
                switch (solution)
                {
                    case "stack":
                        UsingStack(head);
                        break;

                    case "fastslow":
                        UsingFastSlow(head);
                        break;

                    default:
                        throw new NotSupportedException(solution);
                }

                // ---

                for (ListNode? an = head, en = expected; an != null && en != null; an = an.next, en = en.next)
                {
                    Assert.Equal(en.val, an.val);
                    Assert.Equal(inputMap[an.val], an.id);
                }
            }

            /// <summary>
            /// 146. LRU Cache
            /// Design a data structure that follows the constraints of a Least Recently Used(LRU) cache.
            /// Implement the LRUCache class:
            /// LRUCache(int capacity) Initialize the LRU cache with positive size capacity.
            /// int get(int key) Return the value of the key if the key exists, otherwise return -1.
            /// void put(int key, int value) Update the value of the key if the key exists.Otherwise, add the key-value pair to the cache.If the number of keys exceeds the capacity from this operation, evict the least recently used key.
            /// The functions get and put must each run in O(1) average time complexity.
            /// </summary>
            /// <see cref="https://leetcode.com/problems/lru-cache/description/"/>
            [Trait("List", "TopInterview150")]
            [Trait("List", "Facebook")]
            public class LRUCache
            {
                public interface ILRUCache
                {
                    void Put(int key, int value);
                    int Get(int key);
                }

                public class LRUCacheV1 : ILRUCache
                {
                    private class MyNode(int key, int value, MyNode? next = null, MyNode? prev = null)
                    {
                        public readonly int Key = key;
                        public int Value = value;
                        public MyNode? Next = next;
                        public MyNode? Prev = prev;

                        public override string ToString()
                        {
                            return $"({Prev?.Value},{Prev?.Prev})<-[{Key},{Value}]->({Next?.Key},{Next?.Value})";
                        }
                    }

                    private readonly int capacity;
                    private readonly Dictionary<int, MyNode> cache = [];
                    private MyNode? head = null;
                    private MyNode? tail = null;

                    public LRUCacheV1(int capacity)
                    {
                        this.capacity = capacity;
                    }

                    public int Get(int key)
                    {
                        if (!cache.TryGetValue(key, out MyNode? node)) return -1;

                        InternalPromoteToHead(key, node);

                        return node.Value;
                    }

                    public void Put(int key, int value)
                    {
                        if (!cache.TryGetValue(key, out MyNode? node))
                        {
                            // If we are at capacity, we need to make space for the new node
                            if (cache.Count == capacity && InternalTryEvictTail(out int oldKey))
                            {
                                cache.Remove(oldKey);
                            }

                            node = new MyNode(key, value);

                            cache.Add(key, node);
                        }
                        else if (node.Value != value)
                        {
                            // Update value if necessary
                            node.Value = value;
                        }

                        // Update LRU
                        InternalPromoteToHead(key, node);
                    }

                    private void InternalPromoteToHead(int key, MyNode node)
                    {
                        if (head == null)
                        {
                            // Initial state
                            head = node;
                            tail = node;

                            return;
                        }
                        else if (node == head)
                        {
                            // Not necessary
                            return;
                        }
                        else if (node == tail)
                        {
                            // If we are promoting the tail we need to rewind it
                            tail = tail.Prev;
                        }

                        // Detach node from current position
                        if (node.Next != null) node.Next.Prev = node.Prev;
                        if (node.Prev != null) node.Prev.Next = node.Next;

                        // Attach node to current head
                        node.Next = head;
                        head.Prev = node;

                        // Make node the new head
                        head = node;
                        head.Prev = null;
                    }

                    private bool InternalTryEvictTail(out int key)
                    {
                        key = tail?.Key ?? 0;

                        if (tail == null) return false;

                        tail = tail?.Prev;

                        if (tail != null)
                        {
                            tail.Next = null;
                        }
                        else
                        {
                            head = null;
                        }

                        return true;
                    }

                    public IEnumerable<KeyValuePair<int, int>> GetQueue()
                    {
                        for (MyNode? node = head; node != null; node = node?.Next)
                        {
                            yield return new KeyValuePair<int, int>(node.Key, node.Value);
                        }
                    }
                }

                public class LRUCacheV2(int capacity) : ILRUCache
                {
                    private class LRUNode(int key, int value)
                    {
                        public class LRUQueue
                        {
                            private LRUNode? head = null;
                            private LRUNode? tail = null;

                            public void Requeue(LRUNode node)
                            {
                                if (head == null && tail == null)
                                {
                                    head = tail = node;
                                }
                                else if (node != head)
                                {
                                    if (node.prev != null || node.next != null) Detach(node);
                                    AttachBefore(node, head!);
                                }
                            }

                            public LRUNode Dequeue()
                            {
                                LRUNode node = tail!;

                                Detach(node);

                                return node;
                            }

                            public override string ToString()
                            {
                                return $"[{(head?.ToString() ?? "null")}...{(tail?.ToString() ?? "null")}]";
                            }

                            private void AttachBefore(LRUNode node, LRUNode succ)
                            {
                                if (head == succ)
                                {
                                    head = node;
                                }

                                node.next = succ;
                                succ.prev = node;
                            }

                            private void Detach(LRUNode node)
                            {
                                if (head == node)
                                {
                                    head = head.next;
                                }

                                if (tail == node)
                                {
                                    tail = tail.prev;
                                }

                                if (node.prev != null)
                                {
                                    node.prev.next = node.next;
                                }

                                if (node.next != null)
                                {
                                    node.next.prev = node.prev;
                                }

                                node.prev = null;
                                node.next = null;
                            }
                        }

                        public readonly int key = key;
                        public int value = value;
                        private LRUNode? prev;
                        private LRUNode? next;

                        public override string ToString()
                        {
                            return $"{(prev?.key.ToString() ?? "null")}<-({key},{value})->{(next?.key.ToString() ?? "null")}";
                        }
                    }

                    private readonly Dictionary<int, LRUNode> map = new(capacity);
                    private readonly LRUNode.LRUQueue queue = new();

                    public void Put(int key, int value)
                    {
                        if (!map.TryGetValue(key, out var node))
                        {
                            // If we need to add a new node and the cache is at capacity, first we need to drop the queue tail
                            if (map.Count == capacity)
                            {
                                map.Remove(queue.Dequeue().key);
                            }

                            // Create a new node
                            map[key] = node = new(key, value);
                        }
                        else
                        {
                            // We can simply update the value on the existing node
                            node.value = value;
                        }

                        // Update the LRU queue if needed
                        queue.Requeue(node);
                    }

                    public int Get(int key)
                    {
                        if (!map.TryGetValue(key, out var node)) return -1;

                        queue.Requeue(node);

                        return node.value;
                    }
                }

                [Theory]
                [InlineData("[LRUCache,put,put,get,put,get,put,get,get,get]", "[[2],[1,1],[2,2],[1],[3,3],[2],[4,4],[1],[3],[4]]", "[null,null,null,1,null,-1,null,-1,3,4]")]
                [InlineData("[LRUCache,put,put,get,put,get,put,get,get,get]", "[[2],[1,0],[2,2],[1],[3,3],[2],[4,4],[1],[3],[4]]", "[null,null,null,0,null,-1,null,-1,3,4]")]
                [InlineData("[LRUCache,get,get,get,put,get,put,put,put,put,get,put]", "[[1],[1],[6],[8],[12,1],[2],[15,11],[5,2],[1,15],[4,2],[5],[15,15]]", "[null,-1,-1,-1,null,-1,null,null,null,null,-1,null]")]
                [InlineData("[LRUCache,put,put,put,put,get,get,get,get,put,get,get,get,get,get]", "[[3],[1,1],[2,2],[3,3],[4,4],[4],[3],[2],[1],[5,5],[1],[2],[3],[4],[5]]", "[null,null,null,null,null,4,3,2,-1,null,-1,2,3,-1,5]]")]
                public void Test(string cmdsInput, string argsInput, string expsInput)
                {
                    string[] cmds = cmdsInput.Parse1DArray().ToArray();
                    string[][] args = argsInput.Parse2DArray().Select(x => x.ToArray()).ToArray();
                    string[] exps = expsInput.Parse1DArray().ToArray();

                    Assert.Equal(exps.Length, cmds.Length);
                    Assert.Equal(exps.Length, args.Length);


                    string solution = "v2";
                    Func<int, ILRUCache> ctor =
                        solution == "v1" ? c => new LRUCacheV1(c) :
                        solution == "v2" ? c => new LRUCacheV2(c) :
                        throw new NotSupportedException(solution);

                    ILRUCache? lruCache = null;
                    for (int i = 0; i < exps.Length; i++)
                    {
                        switch (cmds[i])
                        {
                            case "LRUCache":
                                lruCache = ctor.Invoke(int.Parse(args[i][0]));
                                break;

                            case "put":
                                lruCache!.Put(int.Parse(args[i][0]), int.Parse(args[i][1]));
                                break;

                            case "get":
                                Assert.Equal(int.Parse(exps[i]), lruCache!.Get(int.Parse(args[i][0])));
                                break;

                            default:
                                throw new NotSupportedException(cmds[0]);
                        }
                    }
                }
            }
        }

        // Given head, the head of a linked list, determine if the linked list has a cycle in it.
        // There is a cycle in a linked list if there is some node in the list that can be reached again by continuously following the next pointer.Internally, pos is used to denote the index of the node that tail's next pointer is connected to. Note that pos is not passed as a parameter.
        // Return true if there is a cycle in the linked list.Otherwise, return false.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[3, 2, 0, -4]", 1, true)]
        [InlineData("[1, 2]", 0, true)]
        [InlineData("[1]", -1, false)]
        public void HasCycle(string inputData, int inputLoop, bool expected)
        {
            ListNode? head = ListNode.ParseFromLC(inputData);
            head?.CreateLoop(inputLoop);

            bool result = false;

            ListNode? ptr1 = head;
            ListNode? ptr2 = head;

            while (ptr1 != null && ptr2 != null)
            {
                ptr1 = ptr1?.next;
                ptr2 = ptr2?.next?.next;

                if (ptr1 == ptr2 && ptr1 != null)
                {
                    result = true;
                    break;
                }
            }

            Assert.Equal(expected, result);
        }

        // Given the head of a singly linked list and two integers left and right where left <= right, reverse the nodes of the list from position left to position right, and return the reversed list.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[1, 2, 3, 4, 5]", 2, 4, "[1,4,3,2,5]")]
        [InlineData("[5]", 1, 1, "[5]")]
        public void ReverseBetween(string input, int left, int right, string output)
        {
            ListNode? head = ListNode.ParseFromLC(input);
            ListNode? expected = ListNode.ParseFromLC(output);

            if (head != null && left < right)
            {
                ListNode dummy = new(0, head);
                ListNode? predNode = dummy;
                for (int i = 1; i < left; i++)
                {
                    predNode = predNode?.next;
                }

                ListNode? leftSwap = predNode?.next;
                ListNode? rightSwap = leftSwap?.next;

                for (int i = left; i < right; i++)
                {
                    leftSwap!.next = rightSwap?.next;
                    rightSwap!.next = predNode?.next;
                    predNode!.next = rightSwap;
                    rightSwap = leftSwap.next;
                }

                head = dummy.next;
            }

            Assert.Equal(expected?.Select(n => n.val) ?? [], head?.Select(n => n.val) ?? []); ;
        }

        // Given the head of a linked list, reverse the nodes of the list k at a time, and return the modified list.
        // k is a positive integer and is less than or equal to the length of the linked list.If the number of nodes is not a multiple of k then left-out nodes, in the end, should remain as it is.
        // You may not alter the values in the list's nodes, only nodes themselves may be changed.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[1,2,3,4,5]", 2, "[2,1,4,3,5]")]
        [InlineData("[1,2,3,4,5]", 3, "[3,2,1,4,5]")]
        public void ReverseKGroup(string headInput, int k, string output)
        {
            ListNode? head = ListNode.ParseFromLC(headInput);
            ListNode? expected = ListNode.ParseFromLC(output);

            ListNode? dummy = new(0, head);
            ListNode? predNode = dummy;

            while (predNode != null)
            {
                // Check if it can be done
                int len = 0;
                for (ListNode? tail = predNode?.next; len < k && tail != null; tail = tail?.next) len++;
                if (len != k) break;

                // Reverse
                ListNode? leftSwap = predNode?.next;
                ListNode? rightSwap = leftSwap?.next;
                for (int i = 1; i < k; i++)
                {
                    leftSwap!.next = rightSwap?.next;
                    rightSwap!.next = predNode?.next;
                    predNode!.next = rightSwap;
                    rightSwap = leftSwap.next;
                }

                // Adjust next baseline
                for (int i = 0; i < k; i++)
                {
                    predNode = predNode?.next;
                }
            }

            head = dummy.next;

            Assert.Equal(expected?.Select(n => n.val) ?? [], head?.Select(n => n.val) ?? []); ;
        }

        // Given the head of a linked list, remove the nth node from the end of the list and return its head.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[1, 2, 3, 4, 5]", 2, "[1, 2, 3, 5]")]
        [InlineData("[1]", 1, "[]")]
        [InlineData("[1, 2]", 1, "[1]")]
        public void RemoveNthFromEnd(string inputHead, int n, string output)
        {
            ListNode? head = ListNode.ParseFromLC(inputHead);
            ListNode? expected = ListNode.ParseFromLC(output);

            static ListNode? InternalRemoveNthFromEnd(ListNode? pred, ref int n)
            {
                if (pred == null) return null;

                pred.next = InternalRemoveNthFromEnd(pred.next, ref n);

                if (n-- == 0) pred.next = pred?.next?.next;

                return pred;
            }

            ListNode? dummy = new(0, head);

            head = InternalRemoveNthFromEnd(dummy, ref n)?.next;

            Assert.Equal(expected?.Select(n => n.val) ?? [], head?.Select(n => n.val) ?? []); ;
        }

        // Given the head of a sorted linked list, delete all nodes that have duplicate numbers, leaving only distinct numbers from the original list. Return the linked list sorted as well.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[1,2,3,3,4,4,5]", "[1,2,5]")]
        [InlineData("[1,1,1,2,3]", "[2,3]")]
        public void DeleteDuplicates(string input, string output)
        {
            ListNode? head = ListNode.ParseFromLC(input);
            ListNode? expected = ListNode.ParseFromLC(output);

            ListNode? dummy = new(0, head);
            for (ListNode? pred = dummy; pred != null;)
            {
                ListNode? skipper = pred?.next;
                while (skipper != null && skipper.val == skipper.next?.val) skipper = skipper.next;

                if (skipper == pred?.next)
                {
                    pred = pred?.next;
                }
                else
                {
                    pred!.next = skipper?.next;
                }
            }

            head = dummy.next;

            Assert.Equal(head?.Select(n => n.val) ?? [], expected?.Select(n => n.val) ?? []);
        }

        // Given the head of a linked list, rotate the list to the right by k places.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[1,2,3,4,5]", 0, "[1,2,3,4,5]")]
        [InlineData("[1,2,3,4,5]", 1, "[5,1,2,3,4]")]
        [InlineData("[1,2,3,4,5]", 2, "[4,5,1,2,3]")]
        [InlineData("[1,2,3,4,5]", 3, "[3,4,5,1,2]")]
        [InlineData("[1,2,3,4,5]", 4, "[2,3,4,5,1]")]
        [InlineData("[1,2,3,4,5]", 5, "[1,2,3,4,5]")]
        [InlineData("[1,2,3,4,5]", 6, "[5,1,2,3,4]")]
        [InlineData("[0,1,2]", 4, "[2,0,1]")]
        [InlineData("[]", 0, "[]")]
        [InlineData("[1,2]", 1, "[2,1]")]
        public void RotateRight(string inputHead, int k, string output)
        {
            ListNode? head = ListNode.ParseFromLC(inputHead);
            ListNode? expected = ListNode.ParseFromLC(output);

            if (head != null && k > 0)
            {
                ListNode? oldTail = head;
                int count = 1;
                for (; oldTail?.next != null; oldTail = oldTail?.next) count++;

                int skip = (count - (k % count)) % count;
                ListNode? newHead = head;
                ListNode? newTail = null;
                for (int i = 0; i < skip; i++)
                {
                    newTail = newHead;
                    newHead = newHead?.next;
                }

                if (newTail != null)
                {
                    oldTail!.next = head;
                    newTail.next = null;
                    head = newHead;
                }
            }

            Assert.Equal(expected?.Select(n => n.val) ?? [], head?.Select(n => n.val) ?? []);
        }

        // Given the head of a linked list and a value x, partition it such that all nodes less than x come before nodes greater than or equal to x.
        // You should preserve the original relative order of the nodes in each of the two partitions.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[1,4,3,2,5,2]", 3, "[1,2,2,4,3,5]")]
        [InlineData("[2,1]", 2, "[1,2]")]
        public void Partition(string inputHead, int x, string output)
        {
            ListNode? head = ListNode.ParseFromLC(inputHead);
            ListNode? expected = ListNode.ParseFromLC(output);

            ListNode? dummy = new(0, head);

            // Find the place where the lower nodes need to be moved to
            ListNode? predTo = dummy;
            while (predTo?.next?.val < x) predTo = predTo?.next;

            ListNode? predFrom = predTo?.next;
            while (predFrom?.next != null)
            {
                // Find a node that is less than the target
                while (predFrom?.next?.val >= x) predFrom = predFrom?.next;

                // If we reached the end of the list, bail out
                if (predFrom?.next == null) continue;

                ListNode? node = predFrom?.next;

                // Remove the node from where it was...
                // [predFrom]->[node]->[rest] => [predFrom]->[rest]
                predFrom!.next = predFrom?.next?.next;

                // Inject the node where it needs to be
                // [predTo]->[rest] => [predTo]->[node]->[rest]
                node!.next = predTo?.next;
                predTo!.next = node;

                // Advance to the next injection point
                predTo = predTo.next;
            }

            head = dummy.next;

            Assert.Equal(expected?.Select(n => n.val) ?? [], head?.Select(n => n.val) ?? []);
        }
    }
}