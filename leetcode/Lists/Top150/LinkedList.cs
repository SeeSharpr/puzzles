﻿using System.Collections;

namespace leetcode.Lists.Top150
{
    public class LinkedList
    {
        private class ListNode(int x, ListNode? next = null) : IEnumerable<ListNode>
        {
            private static int idGen = -1;
            private readonly int id = Interlocked.Increment(ref idGen);

            public readonly int x = x;

            public readonly int val = x;

            public ListNode? next = next;

            public void CreateLoop(int index)
            {
                if (index == -1) return;

                ListNode? ptr = this;
                while (index-- > 0)
                {
                    ptr = ptr!.next;
                }

                ListNode? last = ptr!.next;
                while (last!.next != null)
                {
                    last = last!.next;
                }

                last.next = ptr;
            }

            public override string ToString()
            {
                return $"(id:{id}, val:{val}, next:{next?.id ?? 0}[{next?.val ?? 0}])";
            }

            public IEnumerator<ListNode> GetEnumerator()
            {
                return new NodeEnumerator(this);
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new NodeEnumerator(this);
            }

            public static ListNode? ParseFromLC(string input)
            {
                return input.ParseLinkedListLC(data => new ListNode(data), (node, next) => node.next = next, node => node?.next, int.Parse);
            }

            private class NodeEnumerator(ListNode? head) : IEnumerator<ListNode>
            {
                private readonly int limit = 20;
                private int count = 0;
                private readonly ListNode? head = head;

                private ListNode? prev = null;

                public ListNode Current => prev ?? throw new InvalidOperationException();

                object IEnumerator.Current => prev ?? throw new InvalidOperationException();

                public void Dispose()
                {
                    // Nothing to dispose
                }

                public bool MoveNext()
                {
                    prev = (prev == null) ? head : prev?.next;
                    count++;

                    return prev != null && count < limit;
                }

                public void Reset()
                {
                    prev = null;
                    count = 0;
                }
            }
        }

        private class Node(int val, Node? next = null) : IEnumerable<Node>
        {
            private static int idGen = -1;

            public readonly int id = Interlocked.Increment(ref idGen);
            public int val = val;
            public Node? next = next;
            public Node? random = null;

            public IEnumerator<Node> GetEnumerator()
            {
                return new NodeEnumerator(this);
            }

            public override string ToString()
            {
                return $"(id:{id}, val:{val}, next:{next?.id ?? 0}, random: {random?.id ?? 0})";
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new NodeEnumerator(this);
            }

            private class NodeEnumerator(Node head) : IEnumerator<Node>
            {
                private readonly Node head = head;
                Node? prev = null;

                public Node Current => prev!;

                object IEnumerator.Current => prev!;

                public void Dispose()
                {
                    // Do nothing
                }

                public bool MoveNext()
                {
                    prev = (prev == null) ? head : prev?.next;

                    return prev?.next != null;
                }

                public void Reset()
                {
                    prev = null;
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

        // You are given two non-empty linked lists representing two non-negative integers. The digits are stored in reverse order, and each of their nodes contains a single digit. Add the two numbers and return the sum as a linked list.
        // You may assume the two numbers do not contain any leading zero, except the number 0 itself.
        [Trait("List", "TopInterview150")]
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

        // You are given the heads of two sorted linked lists list1 and list2.
        // Merge the two lists into one sorted list.The list should be made by splicing together the nodes of the first two lists.
        // Return the head of the merged linked list.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[1,2,4]", "[1,3,4]", "[1,1,2,3,4,4]")]
        [InlineData("[]", "[]", "[]")]
        [InlineData("[]", "[0]", "[0]")]
        public void MergeTwoLists(string inputL1, string inputL2, string output)
        {
            ListNode? list1 = ListNode.ParseFromLC(inputL1);
            ListNode? list2 = ListNode.ParseFromLC(inputL2);
            ListNode? expected = ListNode.ParseFromLC(output);

            ListNode? result = null;
            ListNode? next = null;

            while (true)
            {
                if (list1 != null && list2 != null)
                {
                    if (list1.val < list2.val)
                    {
                        next = (result == null) ? (result = new ListNode(list1.val)) : (next.next = new ListNode(list1.val));
                        list1 = list1.next;
                    }
                    else
                    {
                        next = (result == null) ? (next = result = new ListNode(list2.val)) : (next.next = new ListNode(list2.val));
                        list2 = list2.next;
                    }
                }
                else if (list1 != null)
                {
                    next = (result == null) ? (result = new ListNode(list1.val)) : (next.next = new ListNode(list1.val));
                    list1 = list1.next;
                }
                else if (list2 != null)
                {
                    next = (result == null) ? (next = result = new ListNode(list2.val)) : (next.next = new ListNode(list2.val));
                    list2 = list2.next;
                }
                else
                {
                    break;
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

        // A linked list of length n is given such that each node contains an additional random pointer, which could point to any node in the list, or null.
        // Construct a deep copy of the list.The deep copy should consist of exactly n brand new nodes, where each new node has its value set to the value of its corresponding original node.Both the next and random pointer of the new nodes should point to new nodes in the copied list such that the pointers in the original list and copied list represent the same list state.None of the pointers in the new list should point to nodes in the original list.
        // For example, if there are two nodes X and Y in the original list, where X.random --> Y, then for the corresponding two nodes x and y in the copied list, x.random --> y.
        // Return the head of the copied linked list.
        // The linked list is represented in the input/output as a list of n nodes.Each node is represented as a pair of[val, random_index] where:
        // val: an integer representing Node.val
        // random_index: the index of the node (range from 0 to n-1) that the random pointer points to, or null if it does not point to any node.
        // Your code will only be given the head of the original linked list.
        [Trait("List", "TopInterview150")]
        [Theory]
        [InlineData("[[7,-1],[13,0],[11,4],[10,2],[1,0]]")]
        [InlineData("[[1,1],[2,1]]")]
        [InlineData("[[3,-1],[3,0],[3,-1]]")]
        public void CopyRandomList(string input)
        {
            int[][] data = input.ParseNestedArrayStringLC(int.Parse).Select(s => s.ToArray()).ToArray();
            Node? head = data.Select(d => d[0]).Reverse().Aggregate((Node?)null, (next, val) => { Node? node = new(val); node.next = next; return node; });

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

                    ptr.random = random;
                }

                ptr = ptr?.next;
            }

            Dictionary<Node, Node> nodeMap = new();

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

            Node? newHead = head == null ? null : nodeMap[head];

            // Values match
            Assert.Equal(head?.Select(n => n.val) ?? [], newHead?.Select(n => n.val) ?? []);
            // No IDs match for next
            Assert.Empty(head?.Select(x => x.id)?.Where(x => x != null)?.Intersect(newHead?.Select(y => y.id)?.Where(x => x != null)));
            // No IDs match for random
            Assert.Empty(head.Select(x => x.random?.id).Where(x => x != null).Intersect(newHead?.Select(y => y.random?.id)?.Where(x => x != null)));
            // Index of random matches
            int[] oldRandomIndex = head.Select(x => { int index = x.random == null ? -1 : 0; for (Node? p = head; p != null && p != x.random; p = p?.next) index++; return index; }).ToArray();
            int[] newRandomIndex = newHead.Select(x => { int index = x.random == null ? -1 : 0; for (Node? p = newHead; p != null && p != x.random; p = p?.next) index++; return index; }).ToArray();

            Assert.Equal(oldRandomIndex, newRandomIndex);
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
                ListNode predNode = dummy;
                for (int i = 1; i < left; i++)
                {
                    predNode = predNode.next;
                }

                ListNode leftSwap = predNode.next;
                ListNode rightSwap = leftSwap.next;

                for (int i = left; i < right; i++)
                {
                    leftSwap.next = rightSwap.next;
                    rightSwap.next = predNode.next;
                    predNode.next = rightSwap;
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
                    leftSwap.next = rightSwap?.next;
                    rightSwap.next = predNode?.next;
                    predNode.next = rightSwap;
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
                    pred.next = skipper?.next;
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
                    oldTail.next = head;
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
                predFrom.next = predFrom?.next?.next;

                // Inject the node where it needs to be
                // [predTo]->[rest] => [predTo]->[node]->[rest]
                node.next = predTo?.next;
                predTo.next = node;

                // Advance to the next injection point
                predTo = predTo.next;
            }

            head = dummy.next;

            Assert.Equal(expected.Select(n => n.val) ?? [], head.Select(n => n.val) ?? []);
        }

        // Design a data structure that follows the constraints of a Least Recently Used(LRU) cache.
        // Implement the LRUCache class:
        // LRUCache(int capacity) Initialize the LRU cache with positive size capacity.
        // int get(int key) Return the value of the key if the key exists, otherwise return -1.
        // void put(int key, int value) Update the value of the key if the key exists.Otherwise, add the key-value pair to the cache.If the number of keys exceeds the capacity from this operation, evict the least recently used key.
        // The functions get and put must each run in O(1) average time complexity.
        public class LRUCache
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

            public LRUCache(int capacity)
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

        [Trait("List", "TopInterview150")]
        [Fact]
        public void LRUCacheTest()
        {
            KeyValuePair<int, int> p11 = new(1, 1);
            KeyValuePair<int, int> p22 = new(2, 2);
            KeyValuePair<int, int> p33 = new(3, 3);
            KeyValuePair<int, int> p44 = new(4, 4);

            {
                LRUCache lRUCache = new LRUCache(2);

                lRUCache.Put(1, 1); // cache is {1=1}
                Assert.Equal([p11], lRUCache.GetQueue());

                lRUCache.Put(2, 2); // cache is {1=1, 2=2}
                Assert.Equal([p22, p11], lRUCache.GetQueue());

                Assert.Equal(1, lRUCache.Get(1));    // return 1

                lRUCache.Put(3, 3); // LRU key was 2, evicts key 2, cache is {1=1, 3=3}
                Assert.Equal([p33, p11], lRUCache.GetQueue());

                Assert.Equal(-1, lRUCache.Get(2));    // returns -1 (not found)

                lRUCache.Put(4, 4); // LRU key was 1, evicts key 1, cache is {4=4, 3=3}
                Assert.Equal([p44, p33], lRUCache.GetQueue());


                Assert.Equal(-1, lRUCache.Get(1));    // return -1 (not found)

                Assert.Equal(3, lRUCache.Get(3));    // return 3

                Assert.Equal(4, lRUCache.Get(4));    // return 4
            }

            {
                LRUCache lRUCache = new LRUCache(2);
                lRUCache.Put(1, 0);
                lRUCache.Put(2, 2);
                Assert.Equal(0, lRUCache.Get(1));
                lRUCache.Put(3, 3);
                Assert.Equal(-1, lRUCache.Get(2));
                lRUCache.Put(4, 4);
                Assert.Equal(-1, lRUCache.Get(1));
                Assert.Equal(3, lRUCache.Get(3));
                Assert.Equal(4, lRUCache.Get(4));
            }

            {
                LRUCache lRUCache = new LRUCache(1);
                Assert.Equal(-1, lRUCache.Get(1));
                Assert.Equal(-1, lRUCache.Get(6));
                Assert.Equal(-1, lRUCache.Get(8));
                lRUCache.Put(12, 1);
                Assert.Equal(-1, lRUCache.Get(2));
                lRUCache.Put(15, 11);
                lRUCache.Put(5, 2);
                lRUCache.Put(1, 15);
                lRUCache.Put(4, 2);
                Assert.Equal(-1, lRUCache.Get(5));
                lRUCache.Put(15, 15);
            }
        }
    }
}
