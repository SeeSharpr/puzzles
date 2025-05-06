#include "pch.h"
#include "leetcode.h"
#include <tuple>
#include <unordered_set>
#include <vector>


namespace leetcode
{
	TEST_CLASS(arrays_and_strings_easy)
	{
	public:
		BEGIN_TEST_CLASS_ATTRIBUTE()
			TEST_LC_DIFFICULTY(L"Easy")
			END_TEST_CLASS_ATTRIBUTE()
			;

		// 27. Remove Element
		int removeElement(std::vector<int>& nums, int val) {
			auto& newIt = nums.begin();
			for (auto& it = nums.cbegin(); it != nums.cend(); it++)
			{
				if (*it != val) *newIt++ = *it;
			}

			nums.resize(newIt - nums.cbegin());
			return nums.size();

			//int left = 0;
			//int right = 0;
			//for (size_t limit = nums.size(); right < limit; right++)
			//{
			//	if (nums[right] != val)
			//	{
			//		nums[left++] = nums[right];
			//	}
			//}

			//return left;
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

		// 67. Add Binary
		std::string AddBinary(const std::string& a, const std::string& b)
		{
			//int carry = 0;
			//std::string actual;
			//const auto& ia = a.cend();
			//const auto& ib = b.cend();

			//while (ia != a.cbegin() || ib != b.cbegin())
			//{
			//	int va = ia != a.cbegin() ? (*ia - '0') : 0;
			//	int vb = ib != b.cbegin() ? (*ib - '0') : 0;
			//	actual += ((carry ^ va ^ vb) + '0');
			//	carry = (carry & va & vb);
			//}

			//if (carry) actual + '1';
			//actual.swap(std::string(actual.crbegin(), actual.crend()));

			int n = std::max(a.size(), b.size());
			std::vector<char> r(n + 1);
			int cc = 0;

			for (int i = n, ia = a.size() - 1, ib = b.size() - 1; i > 0; i--, ia--, ib--)
			{
				int ca = ia >= 0 ? a[ia] - '0' : 0;
				int cb = ib >= 0 ? b[ib] - '0' : 0;

				r[i] = '0' + ((cc + ca + cb) % 2);
				cc = (cc + ca + cb) / 2;
			}

			r[0] = '0' + cc;

			auto first = r.cbegin();
			if (*first == '0') first++;

			return std::string(first, r.cend());
		}

		TEST_METHOD(test_AddBinary)
		{
			std::vector< std::tuple<std::string, std::string, std::string>> tuples
			{
			{"11", "1", "100"},
			{"1010", "1011", "10101"},
			{"0", "0", "0"},
			{"10100000100100110110010000010101111011011001101110111111111101000000101111001110001111100001101", "110101001011101110001111100110001010100001101011101010000011011011001011101111001100000011011110011", "110111101100010011000101110110100000011101000101011001000011011000001100011110011010010011000000000"},
			};

			for (const std::tuple<std::string, std::string, std::string>& tuple : tuples)
			{
				const auto& a = std::get<0>(tuple);
				const auto& b = std::get<1>(tuple);
				const auto& expected = std::get<2>(tuple);

				Assert::AreEqual(expected, AddBinary(a, b));
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
