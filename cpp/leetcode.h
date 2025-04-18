#pragma once

#include "CppUnitTest.h"

using namespace Microsoft::VisualStudio::CppUnitTestFramework;

#define TEST_LC_COMPANY(traitValue) TEST_CLASS_ATTRIBUTE(L"Company", traitValue)
#define TEST_LC_DIFFICULTY(traitValue) TEST_CLASS_ATTRIBUTE(L"Difficulty", traitValue)

