using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Xml.Linq;
using static leetcode.Lists.Top150.LinkedList;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace leetcode.Lists.Top150
{
    public class LinkedList
    {
        private class ListNode(int x, ListNode? next = null)
        {
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
        }

        // Given head, the head of a linked list, determine if the linked list has a cycle in it.
        // There is a cycle in a linked list if there is some node in the list that can be reached again by continuously following the next pointer.Internally, pos is used to denote the index of the node that tail's next pointer is connected to. Note that pos is not passed as a parameter.
        // Return true if there is a cycle in the linked list.Otherwise, return false.
        [Theory]
        [InlineData("3, 2, 0, -4", 1, true)]
        [InlineData("1, 2", 0, true)]
        [InlineData("1", -1, false)]
        public void HasCycle(string inputData, int inputLoop, bool expected)
        {
            ListNode? head = inputData.ParseArrayStringLC(int.Parse).Reverse().Aggregate((ListNode?)null, (node, value) => new ListNode(value, node));
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
        [Theory]
        [InlineData("2, 4, 3", "5, 6, 4", "7, 0, 8")]
        [InlineData("0", "0", "0")]
        [InlineData("9, 9, 9, 9, 9, 9, 9", "9, 9, 9, 9", "8, 9, 9, 9, 0, 0, 0, 1")]
        public void AddTwoNumbers(string inputL1, string inputL2, string output)
        {
            ListNode? l1 = inputL1.ParseArrayStringLC(int.Parse).Reverse().Aggregate((ListNode?)null, (node, value) => new ListNode(value, node));
            ListNode? l2 = inputL2.ParseArrayStringLC(int.Parse).Reverse().Aggregate((ListNode?)null, (node, value) => new ListNode(value, node));
            ListNode? expected = output.ParseArrayStringLC(int.Parse).Reverse().Aggregate((ListNode?)null, (node, value) => new ListNode(value, node));

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
        [Theory]
        [InlineData("1,2,4", "1,3,4", "1,1,2,3,4,4")]
        [InlineData("", "", "")]
        [InlineData("", "0", "0")]
        public void MergeTwoLists(string inputL1, string inputL2, string output)
        {
            ListNode? list1 = inputL1.ParseArrayStringLC(int.Parse).Reverse().Aggregate((ListNode?)null, (node, value) => new ListNode(value, node));
            ListNode? list2 = inputL2.ParseArrayStringLC(int.Parse).Reverse().Aggregate((ListNode?)null, (node, value) => new ListNode(value, node));
            ListNode? expected = output.ParseArrayStringLC(int.Parse).Reverse().Aggregate((ListNode?)null, (node, value) => new ListNode(value, node));

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
        public class Node : IEnumerable<Node>
        {
            private static int idGen = 0;

            public int val;
            public Node? next;
            public Node? random;
            public readonly int id;

            public Node(int _val)
            {
                val = _val;
                next = null;
                random = null;
                id = Interlocked.Increment(ref idGen);
            }

            public IEnumerator<Node> GetEnumerator()
            {
                return new NodeEnumerator(this);
            }

            public override string ToString()
            {
                return $"(id:{id}, val:{val}, next:{next?.id}, random: {random?.id})";
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return new NodeEnumerator(this);
            }

            private class NodeEnumerator(Node head) : IEnumerator<Node>
            {
                private readonly Node? head = head;
                private bool reset = true;
                private Node? current = head;

                public Node Current => current ?? throw new NullReferenceException();

                object IEnumerator.Current => current ?? throw new NullReferenceException();

                public void Dispose()
                {
                    // Do nothing
                }

                public bool MoveNext()
                {
                    if (reset)
                    {
                        reset = false;
                        return true;
                    }
                    else
                    {
                        current = current?.next;
                        
                        return current != null;
                    }
                }

                public void Reset()
                {
                    current = head;
                    reset = true;
                }
            }
        }

        // A linked list of length n is given such that each node contains an additional random pointer, which could point to any node in the list, or null.
        // Construct a deep copy of the list.The deep copy should consist of exactly n brand new nodes, where each new node has its value set to the value of its corresponding original node.Both the next and random pointer of the new nodes should point to new nodes in the copied list such that the pointers in the original list and copied list represent the same list state.None of the pointers in the new list should point to nodes in the original list.
        // For example, if there are two nodes X and Y in the original list, where X.random --> Y, then for the corresponding two nodes x and y in the copied list, x.random --> y.
        // Return the head of the copied linked list.
        // The linked list is represented in the input/output as a list of n nodes.Each node is represented as a pair of[val, random_index] where:
        // val: an integer representing Node.val
        // random_index: the index of the node (range from 0 to n-1) that the random pointer points to, or null if it does not point to any node.
        // Your code will only be given the head of the original linked list.
        [Theory]
        [InlineData("7,-1|13,0|11,4|10,2|1,0")]
        [InlineData("1,1|2,1")]
        [InlineData("3,-1|3,0|3,-1")]
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
            Assert.True(head?.Select(x => x.val).SequenceEqual(newHead.Select(y => y.val)));
            // No IDs match for next
            Assert.Empty(head?.Select(x => x.id).Where(x => x != null).Intersect(newHead.Select(y => y.id).Where(x => x != null)));
            // No IDs match for random
            Assert.Empty(head.Select(x => x.random?.id).Where(x => x != null).Intersect(newHead.Select(y => y.random?.id).Where(x => x != null)));
            // Index of random matches
            int[] oldRandomIndex = head.Select(x => { int index = x.random == null ? -1 : 0; for (Node? p = head; p != null && p != x.random; p = p?.next) index++; return index; }).ToArray();
            int[] newRandomIndex = newHead.Select(x => { int index = x.random == null ? -1 : 0; for (Node? p = newHead; p != null && p != x.random; p = p?.next) index++; return index; }).ToArray();

            Assert.True(oldRandomIndex.SequenceEqual(newRandomIndex));

        }
    }
}
