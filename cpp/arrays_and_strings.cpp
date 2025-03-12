#include "pch.h"
#include <vector>
#include <tuple>

using std::vector;
using std::tuple;

class ArraysAndStrings {
public:
	static void merge(vector<int>& nums1, int m, vector<int>& nums2, int n) {
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
};

TEST(TestCaseName, TestName) {
	vector<tuple<vector<int>, int, vector<int>, int, vector<int>>> params = {
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

		ArraysAndStrings::merge(nums1, m, nums2, n);

		for (int i = 0; i < nums1.size(); i++)
		{
			ASSERT_EQ(expected[i], nums1[i]);
		}
	}
}