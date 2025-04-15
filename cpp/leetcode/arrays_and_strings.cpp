#include "pch.h"
#include "CppUnitTest.h"
#include <tuple>
#include <unordered_set>
#include <vector>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

#define TEST_LC_DIFFICULTY(traitValue) TEST_CLASS_ATTRIBUTE(L"Difficulty", traitValue)

namespace leetcode
{
	TEST_CLASS(arrays_and_strings_easy)
	{
	public:
		BEGIN_TEST_CLASS_ATTRIBUTE()
			TEST_LC_DIFFICULTY(L"Easy")
		END_TEST_CLASS_ATTRIBUTE()

		// 27. Remove Element
		int removeElement(std::vector<int>& nums, int val) {
		int left = 0;
			int right = 0;
			for (size_t limit = nums.size(); right < limit; right++)
			{
				if (nums[right] != val)
				{
					nums[left++] = nums[right];
				}
			}

			return left;
		}

		TEST_METHOD(test_removeElement)
		{
			std::vector<std::tuple<std::vector<int>, int, std::vector<int>>> params =
			{
				{{3, 2, 2, 3}, 3, {2, 2}},
				{{0, 1, 2, 2, 3, 0, 4, 2}, 2, {0, 1, 3, 0, 4}},
			};

			for (auto& tuple : params)
			{
				auto& nums = std::get<0>(tuple);
				auto& val = std::get<1>(tuple);
				auto& expected = std::get<2>(tuple);

				Assert::AreEqual((int)expected.size(), removeElement(nums, val));
				for (int i = 0; i < expected.size(); i++)
				{
					Assert::AreEqual(expected[i], nums[i]);
				}
			}
		}

		// 88. Merge Sorted Array
		static void merge(std::vector<int>& nums1, int m, std::vector<int>& nums2, int n)
		{
			if (n == 0) return;

			for (int dst = m + n; dst > m;)
			{
				if (m == 0 || nums1[m - 1] < nums2[n - 1])
				{
					nums1[--dst] = nums2[--n];
				}
				else if (n == 0 || nums1[m - 1] >= nums2[n - 1])
				{
					nums1[--dst] = nums1[--m];
				}
			}
		}

		TEST_METHOD(test_merge)
		{
			std::vector<std::tuple<std::vector<int>, int, std::vector<int>, int, std::vector<int>>> params =
			{
				{{1,2,3,0,0,0}, 3, {2,5,6}, 3, {1,2,2,3,5,6}},
				{{1},1,{},0,{1}},
				{{0},0,{1},1,{1}}
			};

			for (auto& tuple : params)
			{
				auto nums1 = std::get<0>(tuple);
				auto m = std::get<1>(tuple);
				auto nums2 = std::get<2>(tuple);
				auto n = std::get<3>(tuple);
				auto expected = std::get<4>(tuple);

				merge(nums1, m, nums2, n);

				for (int i = 0; i < nums1.size(); i++)
				{
					Assert::AreEqual(expected[i], nums1[i]);
				}
			}
		}

		// 2357. Make Array Zero by Subtracting Equal Amounts
		static int minimumOperations(std::vector<int>& nums) {
			std::unordered_set<int> set{ 0 };

			int count = 0;
			for (const int value : nums)
			{
				auto ir = set.insert(value);
				if (ir.second == true) count++;
			}

			return count;
		}

		TEST_METHOD(test_minimumOperations)
		{
			std::vector<std::tuple<std::vector<int>, int>> params =
			{
				{{1,5,0,3,5}, 3},
				{{0}, 0},
			};

			for (auto& tuple : params)
			{
				auto nums = std::get<0>(tuple);
				auto expected = std::get<1>(tuple);

				Assert::AreEqual(expected, minimumOperations(nums));
			}

		}
	};
}
