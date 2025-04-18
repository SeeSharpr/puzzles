#include "pch.h"
#include <deque>

// 27. Remove Element
class removeElement : public testing::TestWithParam<std::tuple<std::vector<int>, int, std::vector<int>>> {};

TEST_P(removeElement, removeElement)
{
	auto nums = std::get<0>(GetParam());
	const auto val = std::get<1>(GetParam());
	const auto expected = std::get<2>(GetParam());

	auto itNew = nums.begin();
	for (auto it = nums.begin(); it != nums.end(); it++)
	{
		if (*it != val) *itNew++ = *it;
	}
	nums.resize(std::distance(nums.begin(), itNew));

	ASSERT_EQ(expected, nums);
}

INSTANTIATE_TEST_CASE_P(
	_,
	removeElement,
	::testing::Values(
		std::make_tuple(std::vector<int>{3, 2, 2, 3}, 3, std::vector<int>{2, 2}),
		std::make_tuple(std::vector<int>{0, 1, 2, 2, 3, 0, 4, 2}, 2, std::vector<int>{0, 1, 3, 0, 4})
	));

// 67. Add Binary
class addBinary : public testing::TestWithParam < std::tuple<std::string, std::string, std::string>> {};

TEST_P(addBinary, addBinary)
{
	const auto& a = std::get<0>(GetParam());
	const auto& b = std::get<1>(GetParam());
	const auto& expected = std::get<2>(GetParam());


	std::deque<char> result;

	int c = 0;
	for (auto& ia = a.crbegin(), ib = b.crbegin(); ia != a.crend() || ib != b.crend(); )
	{
		int va = ia != a.crend() ? *ia++ - '0' : 0;
		int vb = ib != b.crend() ? *ib++ - '0' : 0;

		result.push_front((c + va + vb) % 2 + '0');
		c = (c + va + vb) / 2;
	}

	if (c > 0) result.push_front('1');

	std::string actual(result.cbegin(), result.cend());

	ASSERT_EQ(expected, actual);
}

INSTANTIATE_TEST_CASE_P(
	_
	, addBinary,
	::testing::Values(
		std::make_tuple("11", "1", "100"),
		std::make_tuple("1010", "1011", "10101"),
		std::make_tuple("0", "0", "0"),
		std::make_tuple("10100000100100110110010000010101111011011001101110111111111101000000101111001110001111100001101", "110101001011101110001111100110001010100001101011101010000011011011001011101111001100000011011110011", "110111101100010011000101110110100000011101000101011001000011011000001100011110011010010011000000000")
	));