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

    # 169. Majority Element
    def majorityElement(self, nums: List[int]) -> int:
        solution="sorting"
        match solution:
            case "voting":
                # Boyer-Moore Voting Algorithm
                cand = None
                count = 0

                for num in nums:
                    if (count == 0):
                        cand = num

                    count += 1 if cand == num else -1
                    
            case "sorting":
                # Sort input, the majority element will necessarily be in the center
                nums.sort()
                cand = nums[len(nums)//2]

            case _:
                cand = None

        return cand

    def test_majorityElement(self):
        self.assertEqual(self.majorityElement([3,2,3]), 3)
        self.assertEqual(self.majorityElement([2,2,1,1,1,2,2]), 2)

    # 189. Rotate Array
    def rotate(self, nums: List[int], k: int) -> None:
        limit = len(nums)
        k = k % limit
        if (k == 0):
            return
        
        # Reverse the first K elements
        left = 0
        right = limit - k - 1
        while (left < right):
            nums[left], nums[right] = nums[right], nums[left]
            left, right = left+1, right-1

        # Reverse the last limit-K elements
        left = limit - k
        right = limit-1
        while (left < right):
            nums[left], nums[right] = nums[right], nums[left]
            left, right = left+1, right-1

        # Reverse the whole thing
        left = 0
        right = limit-1
        while (left < right):
            nums[left], nums[right] = nums[right], nums[left]
            left, right = left+1, right-1

    def test_rotate(self):
        nums = [1,2,3,4,5,6,7]
        k = 3
        self.rotate(nums, k)
        self.assertSequenceEqual(nums, [5,6,7,1,2,3,4])

        nums = [-1,-100,3,99]
        k = 2
        self.rotate(nums, k)
        self.assertSequenceEqual(nums, [3,99,-1,-100])

    # 121. Best Time to Buy and Sell Stock
    def maxProfit(self, prices: List[int]) -> int:
        minPrice=-1
        maxProfit=0
        for i in range(len(prices)):
            if (minPrice == -1 or prices[i] < minPrice):
                minPrice = prices[i]
            
            if (prices[i] - minPrice > maxProfit):
                maxProfit = prices[i] - minPrice
        
        return 0 if maxProfit == None else maxProfit

    def test_maxProfit(self):
        self.assertEqual(self.maxProfit([7,1,5,3,6,4]), 5)
        self.assertEqual(self.maxProfit([7,6,4,3,1]), 0)

    # 122. Best Time to Buy and Sell Stock II
    def maxProfit2(self, prices: List[int]) -> int:
        if (len(prices) < 1):
            return 0
        
        profit = 0
        i = 1
        solution="daytrader"

        match solution:
            case "peaksandvalleys":
                while (i < len(prices)):
                    while (i < len(prices) and prices[i] <= prices[i-1]):
                        i += 1
                    valley = i-1
                    
                    while ( i < len(prices) and prices[i] >= prices[i-1]):
                        i += 1
                    peak = i-1

                    if (peak < len(prices) and valley < len(prices)):
                        profit += prices[peak] - prices[valley]
            case "daytrader":
                for i in range(1,len(prices)):
                    profit += max(0, prices[i]-prices[i-1])

        return profit

    def test_maxProfit2(self):
        self.assertEqual(self.maxProfit2([7,1,5,3,6,4]), 7)
        self.assertEqual(self.maxProfit2([1,2,3,4,5]), 4)
        self.assertEqual(self.maxProfit2([7,6,4,3,1]), 0)
        self.assertEqual(self.maxProfit2([3,3]), 0)

    # 55. Jump Game
    def canJump(self, nums: List[int]) -> bool:
        result = True
        remainingJumps=0
        for i in range(len(nums)-1):
            # Greedy approach - We are not trying to optimize the number of jumps, so all we care is whether we can avoid landing on a 0 with no jumps
            # To do so, all we need to do is to take a jump of a distance that is the maximum between what the current place allows and what we have left minus the current place
            # We also don't care about the last place, since it's the destination, all we care is whether we don't stop at a 0 right before the last
            remainingJumps = max(remainingJumps-1, nums[i])
            if (remainingJumps == 0):
                result = False
                break
        return result

    def test_canJump(self):
        self.assertEqual(self.canJump([2,3,1,1,4]), True)
        self.assertEqual(self.canJump([3,2,1,0,4]), False)

    # 45. Jump Game II
    def jump(self, nums: List[int]) -> int:
        solution="greedy"
        match solution:
            case "mine":
                map = {}
                map[0] = 0
                target = len(nums)-1
                for curr in range(target):
                    # Skip over bad jumping points
                    if (nums[curr] == 0):
                        continue

                    jump_count = map[curr]
                    maxJump=min(nums[curr], target-curr)
                    for length in range(maxJump, 0, -1):
                        next = curr + length

                        # Skip over bad landing points
                        if (next != target and nums[next] == 0):
                            continue

                        if (next in map):
                            # We can reach dst from src with a certain cost that is either the previous or the current+1
                            map[next] = min(map[next], jump_count + 1)
                        else:
                            # We can reach dst from src for the first time
                            map[next] = jump_count + 1
                
                return map[target] if target in map else 0
            case "greedy":
                jump_count, window_start, window_end, goal = 0, 0, 0, len(nums) - 1
                for i in range(goal):
                    window_end = max(window_end, i + nums[i])
                    if (i == window_start):
                        jump_count += 1
                        window_start = window_end
                return jump_count if window_end >= goal else 0
            case _:
                raise NotImplementedError(solution)

    def test_jump(self):
        self.assertEqual(self.jump([2,3,1,1,4]),2)
        self.assertEqual(self.jump([2,3,0,1,4]),2)
        self.assertEqual(self.jump([0]), 0)
        self.assertEqual(self.jump([5,9,3,2,1,0,2,3,3,1,0,0]), 3)
        self.assertEqual(self.jump([1,2]), 1)
        self.assertEqual(self.jump([5,4,3,2,2,0,0]), 2)

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