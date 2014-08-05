from collections import defaultdict
__author__ = 'befulton'

def floyd(graph):
    dist = defaultdict(lambda: defaultdict(lambda: 1000000))
    for v in graph.keys():
        dist[v][v] = 0
    for u, values in graph.iteritems():
        for v in values:
            dist[u][v[0]] = v[1]
    for k in graph.keys():
        for i in graph.keys():
            for j in graph.keys():
                if dist[i][j] > dist[i][k] + dist[k][j]:
                    dist[i][j] = dist[i][k] + dist[k][j]
    return dist
