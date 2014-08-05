__author__ = 'befulton'

import unittest
import FloydWarshall
from itertools import product

class TestFloydWarshall(unittest.TestCase):

    def test_floyd(self):
        graph = dict()
        graph['S'] = [('U', 10), ('X', 5)]
        graph['U'] = [('X', -2), ('V', 1)]
        graph['V'] = [('Y', -5)]
        graph['X'] = [('U', 3), ('Y', 2)]
        graph['Y'] = [('V', 6), ('S', 7)]
        dist = FloydWarshall.floyd(graph)
        self.assertEquals(0, dist['S']['S'])
        self.assertEquals(0, dist['U']['U'])
        self.assertEquals(0, dist['V']['V'])
        self.assertEquals(0, dist['X']['X'])
        self.assertEquals(1, dist['U']['V'])
        self.assertEquals(-4, dist['U']['Y'])
        self.assertEquals(8, dist['S']['U'])

        diameter = max(product(dist.keys(), dist.keys()), key=lambda x: dist[x[0]][x[1]])
        self.assertEquals(('Y', 'U'), diameter)
        self.assertEquals(15, dist[diameter[0]][diameter[1]])
