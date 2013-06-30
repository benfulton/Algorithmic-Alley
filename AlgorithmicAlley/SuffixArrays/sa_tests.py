import unittest
import sa
import random


def random_word(length):
    return ''.join(random.choice("AGCT") for i in range(length))


class TestSuffixArrays(unittest.TestCase):

    def test_simple(self):
        self.assertEquals([10, 7, 4, 1, 0, 9, 8, 6, 3, 5, 2], sa.get_suffix_array("mississippi"))

    def test_mm(self):
        for i in range(50):
            str = random_word(1000)
            x = sa.get_suffix_array(str)
            y = sa.suffix_array_ManberMyers(str)
            self.assertEquals(x,y)
