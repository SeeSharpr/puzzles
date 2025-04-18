#include "pch.h"
#include "leetcode.h"
#include <unordered_map>

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

namespace leetcode
{
	TEST_CLASS(linked_lists_medium)
	{
	private:
		class Node {
		public:
			int val;
			Node* next;
			Node* random;

			Node(int _val) {
				val = _val;
				next = NULL;
				random = NULL;
			}
		};
	public:
		BEGIN_TEST_CLASS_ATTRIBUTE()
			TEST_LC_DIFFICULTY(L"Medium")
		END_TEST_CLASS_ATTRIBUTE()

		// 138. Copy List with Random Pointer
		static Node* copyRandomList(Node* head) {
			std::unordered_map<Node*, Node*> map;

			// Clone each node
			for (Node* ptr = head; ptr != nullptr; ptr = ptr->next)
			{
				map.insert(std::make_pair(ptr, new Node(ptr->val)));
			}

			// Link new nodes
			for (Node* ptr = head; ptr != nullptr; ptr = ptr->next)
			{
				map[ptr]->next = map[ptr->next];
				map[ptr]->random = map[ptr->random];
			}

			return map[head];
		}

		static std::string reorganizeString(std::string s) {
			std::string r;

			std::unordered_map<char, int> bag;
			for (const char& c : s)
			{
				auto it = bag.find(c);
				if (it == bag.end())
				{
					bag.insert(std::make_pair(c, 1));
				}
				else
				{
					*it++;
				}
			}
		}
	};
};