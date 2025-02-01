using Microsoft.VisualStudio.TestPlatform.Utilities;
using System;
using System.Xml.Linq;

namespace leetcode.Lists.Top150
{
    public class LinkedList
    {
        private class ListNode(int x, ListNode? next = null)
        {
            public int x { get; } = x;
            public ListNode? next { private set; get; } = next;

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
    }
}
