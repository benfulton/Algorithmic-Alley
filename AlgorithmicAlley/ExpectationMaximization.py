from __future__ import division

rolls = [ "HTTTHHTHTH",
          "HHHHTHHHHH",
          "HTHHHHHTHH",
          "HTHTTTHHTT",
          "THHHTHHHTH" ]

theta = { "A":.6, "B":.5 }

def likelihoodOfRoll( roll, prob ):
    numHeads = roll.count( "H" )
    return pow( prob, numHeads ) * pow( 1-prob, len(roll) - numHeads )

while True:
    headSum = { "A":0, "B":0 }
    tailSum = { "A":0, "B":0 }
    for roll in rolls:
        s = sum( likelihoodOfRoll( roll, k) for k in theta.values())
        for hidden in theta.keys():
            a = likelihoodOfRoll( roll, theta[hidden])
            aChance = a / s
            headSum[hidden] += roll.count( "H" ) * aChance
            tailSum[hidden] += roll.count( "T" ) * aChance

    last = sum( theta.values())
    for hidden in theta.keys():
        theta[hidden] = headSum[hidden] / (headSum[hidden] + tailSum[hidden])

    print theta

    if abs( sum( theta.values()) - last ) < .00001:
        break
    
