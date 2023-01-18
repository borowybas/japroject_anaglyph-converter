#pragma once
#include "pch.h"
extern "C" int __declspec(dllexport) addNumbers(int a, int b) {
    return a + b;
}