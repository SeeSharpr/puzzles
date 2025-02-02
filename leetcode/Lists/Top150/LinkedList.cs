using System.Reflection;
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
    }
}
