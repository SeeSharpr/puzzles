from typing import List
import unittest

class TestArraysAndStrings(unittest.TestCase):
    # 88. Merge Sorted Array
    def merge(self, nums1: List[int], m: int, nums2: List[int], n: int) -> None:
        if (n == 0):
            return

        dst = m+n
        while (dst > m):
            if (m == 0 or nums1[m - 1] < nums2[n - 1]):
                dst -= 1
                n -= 1
                nums1[dst] = nums2[n]
            elif (n == 0 or nums1[m - 1] >= nums2[n - 1]):
                dst -= 1
                m -= 1
                nums1[dst] = nums1[m]

    def test_merge(self):
        nums1=[1,2,3,0,0,0]
        self.merge(nums1,3, [2,5,6], 3)
        self.assertEqual(nums1,[1,2,2,3,5,6])

        nums1=[1]
        self.merge(nums1, 1, [], 0)
        self.assertEqual(nums1, [1])

        nums1=[0]
        self.merge(nums1, 0, [1], 1)
        self.assertEqual(nums1, [1])

    # 27. Remove Element
    def removeElement(self, nums: List[int], val: int) -> int:
        left = right = 0
        limit = len(nums)
        while (right < limit):
            if (nums[right] != val):
                nums[left] = nums[right]
                left += 1
            right += 1

        return left

    def test_removeElement(self):
        nums = [3,2,2,3]
        val = 3
        self.assertEqual(self.removeElement(nums, val), 2)
        self.assertSequenceEqual(nums[0:2], [2,2])

        nums = [0,1,2,2,3,0,4,2]
        val = 2
        self.assertEqual(self.removeElement(nums, val), 5)
        self.assertSequenceEqual(nums[0:5], [0,1,3,0,4])

    # 26. Remove Duplicates from Sorted Array
    def removeDuplicates(self, nums: List[int]) -> int:
        limit = len(nums)

        left = 0
        for right in range(1,limit):
            if (nums[left] != nums[right]):
                left += 1
                nums[left] = nums[right]

        if (limit > 0):
            left += 1

        return left

    def test_removeDuplicates(self):
        nums = [1,1,2]
        self.assertEqual(self.removeDuplicates(nums),2)
        self.assertSequenceEqual(nums[:2], [1,2])

        nums = [0,0,1,1,1,2,2,3,3,4]
        self.assertEqual(self.removeDuplicates(nums),5)
        self.assertSequenceEqual(nums[:5], [0,1,2,3,4])

        nums = []
        self.assertEqual(self.removeDuplicates(nums), 0)
        self.assertSequenceEqual(nums, [])

    # 80. Remove Duplicates from Sorted Array II
    def removeDuplicatesII(self, nums: List[int]) -> int:
        limit = len(nums)

        copies=1
        left = 0
        for right in range(1,limit):
            is_different = nums[left] != nums[right]
            if (is_different or copies < 2):
                if (is_different):
                    copies = 1
                else:
                    copies += 1
                left +=1
                nums[left] = nums[right]
            right += 1

        if (limit > 0):
            left += 1

        return left
    
    def test_removeDuplicatesII(self):
        nums=[1,1,1,2,2,3]
        self.assertEqual(self.removeDuplicatesII(nums), 5)
        self.assertSequenceEqual(nums[:5], [1,1,2,2,3])

        nums=[0,0,1,1,1,1,2,3,3]
        self.assertEqual(self.removeDuplicatesII(nums), 7)
        self.assertSequenceEqual(nums[:7], [0,0,1,1,2,3,3])

    # 1. Two-sum
    def twoSum(self, nums: List[int], target: int) -> List[int]:
        map = {}
        for index,num in enumerate(nums):
            if (target-num in map):
                return [map[target-num], index]
            else:
                map[num] = index
        return []            

    def test_twoSum(self):
        self.assertEqual(self.twoSum([3,2,4], 6),[1,2])
        self.assertEqual(self.twoSum([2,7,11,15], 9), [0,1])
        self.assertEqual(self.twoSum([3,3], 6), [0,1])

if __name__ == '__main__':
    unittest.main(verbosity=2)