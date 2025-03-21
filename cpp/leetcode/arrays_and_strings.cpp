#include "pch.h"
#include "CppUnitTest.h"
#include <tuple>
#include <vector>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;
using std::get;
using std::tuple;
using std::vector;

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
		int removeElement(vector<int>& nums, int val) {
			int left = 0;
			int right = 0;
			for (int limit = nums.size(); right < limit; right++)
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
			vector<tuple<vector<int>, int, vector<int>>> params =
			{
				{{3, 2, 2, 3}, 3, {2, 2}},
				{{0, 1, 2, 2, 3, 0, 4, 2}, 2, {0, 1, 3, 0, 4}},
			};

			for (auto& tuple : params)
			{
				auto& nums = get<0>(tuple);
				auto& val = get<1>(tuple);
				auto& expected = get<2>(tuple);

				Assert::AreEqual((int)expected.size(), removeElement(nums, val));
				for (int i = 0; i < expected.size(); i++)
				{
					Assert::AreEqual(expected[i], nums[i]);
				}
			}
		}

		// 88. Merge Sorted Array
		static void merge(vector<int>& nums1, int m, vector<int>& nums2, int n)
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
			vector<tuple<vector<int>, int, vector<int>, int, vector<int>>> params =
			{
				{{1,2,3,0,0,0}, 3, {2,5,6}, 3, {1,2,2,3,5,6}},
				{{1},1,{},0,{1}},
				{{0},0,{1},1,{1}}
			};

			for (auto& tuple : params)
			{
				auto nums1 = get<0>(tuple);
				auto m = get<1>(tuple);
				auto nums2 = get<2>(tuple);
				auto n = get<3>(tuple);
				auto expected = get<4>(tuple);

				merge(nums1, m, nums2, n);

				for (int i = 0; i < nums1.size(); i++)
				{
					Assert::AreEqual(expected[i], nums1[i]);
				}
			}
		}
	};
}
