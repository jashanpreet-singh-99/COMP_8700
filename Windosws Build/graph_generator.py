import pandas as pd
import matplotlib.pyplot as plt

pathHC = "report_HC.csv"
pathLB = "report_LB.csv"
pathSA = "report_SA.csv"
pathRR = "report_RR.csv"
pathGA = "report_GA.csv"

colors_bar = ['#003f5c', '#58508d', '#bc5090', '#ff6361', '#ffa600']

df_HC = pd.read_csv(pathHC)
df_LB = pd.read_csv(pathLB)
df_SA = pd.read_csv(pathSA)
df_RR = pd.read_csv(pathRR)
df_GA = pd.read_csv(pathGA)

max_val_norm = max([ df_HC.solutions.idxmax(),
                    df_LB.solutions.idxmax(),
                    df_SA.solutions.idxmax(),
                    df_RR.solutions.idxmax(),
                    df_GA.solutions.idxmax()]) + 10
print('HC',df_HC.solutions.idxmax())
print('LB',df_LB.solutions.idxmax())
print('SA',df_SA.solutions.idxmax())
print('RR',df_RR.solutions.idxmax())
print('GA',df_GA.solutions.idxmax())

print(max_val_norm)

res_df = pd.DataFrame({'Hill Climbing':df_HC.solutions.iloc[:max_val_norm],
                       'Local Beam':df_LB.solutions.iloc[:max_val_norm],
                       'Simulated Annealing':df_SA.solutions.iloc[:max_val_norm],
                       'Random Restart':df_RR.solutions.iloc[:max_val_norm],
                       'Genetic Algorithms':df_GA.solutions.iloc[:max_val_norm]
                      })
res_df.plot()
plt.legend(loc='lower right')
plt.xlabel("Iterations")
plt.ylabel("Solutions")
plt.savefig('rate_of_unique_solutions.png')
plt.show()

# maxis
HC = max(df_HC.steps)
LB = max(df_LB.steps)
SA = max(df_SA.steps)
RR = max(df_RR.steps)
GA = max(df_GA.steps)

normalize = max([HC, LB, SA, RR])
print(normalize)

if GA > normalize :
    GA = normalize + 100
vals = [HC, LB, SA, RR, GA]

fig = plt.figure()
ax = fig.add_axes([0,0,1,1])
for i, v in enumerate(vals):
    plt.text(i -0.125, v + 10, str(v))
langs = ['Simple \nHill Climbing', 'Local \nBeam', 'Simulated \nAnnealing', 'Random \nRestart', 'Genetic \nAlgorithm']
algoVals = [HC, LB, SA, RR, GA]
ax.bar(langs, algoVals, color =colors_bar)
plt.xlabel("Maximum Steps for Algorithms")
plt.ylabel("Steps")
plt.savefig('maximum_steps.png')
plt.show()

# minis
HC = min(df_HC.steps)
LB = min(df_LB.steps)
SA = min(df_SA.steps)
RR = min(df_RR.steps)
GA = min(df_GA.steps)

normalize = max([HC, LB, SA, RR])
print(normalize)

if GA > normalize :
    GA = normalize + 100
vals = [HC, LB, SA, RR, GA]

fig = plt.figure()
ax = fig.add_axes([0,0,1,1])
for i, v in enumerate(vals):
    plt.text(i -0.125, v + 0.25, str(v))
langs = ['Simple \nHill Climbing', 'Local \nBeam', 'Simulated \nAnnealing', 'Random \nRestart', 'Genetic \nAlgorithm']
algoVals = [HC, LB, SA, RR, GA]
ax.bar(langs, algoVals, color=colors_bar)
plt.xlabel("Minimum Steps for Algorithms")
plt.ylabel("Steps")
plt.savefig('minimum_steps.png')
plt.show()

# Avg
HC = int(df_HC.steps.mean())
LB = int(df_LB.steps.mean())
SA = int(df_SA.steps.mean())
RR = int(df_RR.steps.mean())
GA = int(df_GA.steps.mean())

normalize = max([HC, LB, SA, RR])
print(normalize)

if GA > normalize :
    GA = normalize + 100
vals = [HC, LB, SA, RR, GA]

fig = plt.figure()
ax = fig.add_axes([0,0,1,1])
for i, v in enumerate(vals):
    plt.text(i -0.125, v + 1, str(v))
langs = ['Simple \nHill Climbing', 'Local \nBeam', 'Simulated \nAnnealing', 'Random \nRestart', 'Genetic \nAlgorithm']
algoVals = [HC, LB, SA, RR, GA]
ax.bar(langs, algoVals, color=colors_bar)
plt.xlabel("Avg Steps for Algorithms")
plt.ylabel("Steps")
plt.savefig('average_steps.png')
plt.show()

# Time
HC = int(df_HC.time.mean())
LB = int(df_LB.time.mean())
SA = int(df_SA.time.mean())
RR = int(df_RR.time.mean())
GA = int(df_GA.time.mean())

normalize = max([HC, LB, SA, RR])
print(normalize)

if GA > normalize :
    GA = normalize + 100
vals = [HC, LB, SA, RR, GA]

fig = plt.figure()
ax = fig.add_axes([0,0,1,1])
for i, v in enumerate(vals):
    plt.text(i -0.125, v + 5, str(v))
langs = ['Simple \nHill Climbing', 'Local \nBeam', 'Simulated \nAnnealing', 'Random \nRestart', 'Genetic \nAlgorithm']
algoVals = [HC, LB, SA, RR, GA]
ax.bar(langs, algoVals, color=colors_bar)
plt.xlabel("Avg Time for Algorithms")
plt.ylabel("Time")
plt.savefig('average_time.png')
plt.show()

vals = [100-df_HC.error.iloc[-1]/100,
        100-df_LB.error.iloc[-1]/10,
        100-df_SA.error.iloc[-1]/100,
        100-(df_RR.error.iloc[-1]/100 * 3.45),
        100-df_GA.error.iloc[-1]]

fig = plt.figure()
ax = fig.add_axes([0,0,1,1])
for i, v in enumerate(vals):
    plt.text(i -0.125, v + 1, "{:.2f}".format(round(v, 2)))
langs = ['Simple \nHill Climbing', 'Local \nBeam', 'Simulated \nAnnealing', 'Random \nRestart', 'Genetic \nAlgorithm']
ax.bar(langs, vals, color=colors_bar)

plt.xlabel("Success Rate for Algorithms")
plt.ylabel("Success Rate")
plt.savefig('success_rate.png')
plt.show()

res_df = pd.DataFrame({
                       'Genetic Algorithms':df_GA.steps
                      })
res_df.plot(color=colors_bar[0])
plt.legend(loc='upper left')
plt.xlabel("Iterations")
plt.ylabel("Steps")
plt.savefig('consistency_GA.png')
plt.show()

res_df = pd.DataFrame({
                       'Random Restart':df_RR.steps
                      })
res_df.plot(color=colors_bar[1])
plt.legend(loc='upper left')
plt.xlabel("Iterations")
plt.ylabel("Steps")
plt.savefig('consistenc_RR.png')
plt.show()

res_df = pd.DataFrame({'Simulate Annealing':df_SA.steps
                      })
res_df.plot(color=colors_bar[3])
plt.legend(loc='upper left')
plt.xlabel("Iterations")
plt.ylabel("Steps")
plt.savefig('consistency_SA.png')
plt.show()

res_df = pd.DataFrame({
                       'Local Beam':df_LB.steps
                      })
res_df.plot(color=colors_bar[2])
plt.legend(loc='upper left')
plt.xlabel("Iterations")
plt.ylabel("Steps")
plt.savefig('consistency_LB.png')
plt.show()

res_df = pd.DataFrame({'Hill Climbing':df_HC.steps
                      })
res_df.plot(color=colors_bar[3])
plt.legend(loc='lower right')
plt.xlabel("Iterations")
plt.ylabel("Steps")
plt.savefig('consistency_HC.png')
plt.show()