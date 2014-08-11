from collections import defaultdict
from itertools import chain
import FloydWarshall
import random

def kmers_in_word(w):
    return [w[i:i + 4] for i in range(len(w) - 4)]

def random_path(d):
    result = []
    items = d.keys()
    while len(items) > 0:
        x = random.choice(items)
        result.append(x)
        items = list(d[x])
    return result

def branch(d, s, root, level=0):
    if level < 3:
        root[0].append(s)
        root[1].extend((s, edge, next(k for k in kmers[s] if edge in k)) for edge in d[s])
        for edge in d[s]:
            branch(d, edge, root, level+1)


def words_sharing_kmer(word, d):
    return chain(d[k] for k in kmers_in_word(word))

if __name__ == '__main__':
    with open("corncob_lowercase.txt") as f:
        s = (line.strip() for line in f)
        word_list = [w for w in s if len(w) > 3]

    kmers = defaultdict(list)
    for word in word_list:
        for k in kmers_in_word(word):
            kmers[k].append(word)

    debruijn = defaultdict(set)
    for kmer, words in kmers.iteritems():
        for word in words:
            for k in kmers_in_word(word):
                if kmer[1:4] == k[0:3]:
                    debruijn[kmer].add(k)
    print len(debruijn)
   # print debruijn

#    p = random_path(debruijn)
#    while len(p) < 10:
#        p = random_path(debruijn)
#    print p

#    for i in range(len(p)-1):
#        print [k for k in kmers[p[i]] if p[i+1] in k]

tree = ([],[])
branch(debruijn, 'rati', tree)
for edge in tree[1]:
    print ",".join(edge)

