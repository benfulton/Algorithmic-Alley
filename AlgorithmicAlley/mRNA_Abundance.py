from __future__ import division

__author__ = 'befulton'
import random
from collections import Counter, defaultdict
import unittest

all_words = ['ingratiate', 'gratification', 'aristocratic', 'aromaticity',
'palatial', 'beatific', 'beatify', 'latices', 'insatiable', 'artifice',
'beatified', 'certifiable']

def kmers_in_word(w):
    return [w[i:i + 4] for i in range(len(w) - 4)]

def get_frequencies(assigned_reads):
    total = sum(len(reads) for reads in assigned_reads.values())
    rmc = dict()
    for word, reads in assigned_reads.iteritems():
        rmc[word] = len(reads) / total
    return rmc

def assign_reads(kmers):
    d = defaultdict(list)
    for read in kmers:
        words = [word for word in all_words if read in word]
        d[random.choice(words)].append(read)
    return d

def rpkm(word, kmers, assigned_reads):
    word_bias = len(assigned_reads[word]) / len(word)
    return int(word_bias * 1000000000 / len(kmers))

def tpm(word, assigned_reads):
    t_val = T(word, assigned_reads)
    (len(assigned_reads[word]) * 4) / (len(word) * t_val)

def T_proxy(word, assigned_reads):
    return (len(assigned_reads[word]) * 4) / len(word)

def T(words, kmers):
    return sum(T_proxy(word, kmers) for word in words)

class testMRNA(unittest.TestCase):
    def setUp(self):
        expressed_words = ['ingratiate', 'aristocratic', 'aristocratic', 'aromaticity',
        'palatial', 'beatific', 'beatify', 'latices', 'insatiable', 'artifice',
        'beatified', 'aristocratic']

        s = set(kmer for w in expressed_words for kmer in kmers_in_word(w))
        kmers = list(s)
        random.seed(100)
        self.fragments = [random.choice(kmers) for i in range(100)]

    def test_assign_reads(self):
        assignments = assign_reads(['ingr'])
        self.assertEquals(dict(ingratiate=['ingr']), assignments)

    def test_get_frequencies(self):
        f = get_frequencies(assign_reads(self.fragments))
        #self.assertAlmostEquals(0.03, f['certifiable'], 2)
        self.assertAlmostEquals(0.2, f['aristocratic'], 2)
        self.assertAlmostEquals(0.15, f['ingratiate'], 2)
        self.assertAlmostEquals(0.03, f['palatial'], 2)

    def test_rpkm(self):
        a = assign_reads(self.fragments)
        #self.assertEquals(4545454, rpkm('certifiable', self.fragments, a))
        self.assertEquals(16666666, rpkm('aristocratic', self.fragments, a))
        self.assertEquals(8571428, rpkm('latices', self.fragments, a))

