# Bench results

## Original

|     Method |    Job | BuildConfiguration |    Size |          Mean |      Error |     StdDev | Rank |   Gen 0 |  Gen 1 |  Gen 2 | Allocated | Code Size |
|----------- |------- |------------------- |-------- |--------------:|-----------:|-----------:|-----:|--------:|-------:|-------:|----------:|----------:|
|    getItem |  After |         LocalBuild |   10000 |      72.75 ns |   0.128 ns |   0.253 ns |    1 |       - |      - |      - |         - |     288 B |
|    getItem | Before |            Default |   10000 |     126.84 ns |   0.224 ns |   0.443 ns |    3 |       - |      - |      - |         - |     129 B |
|    getItem |  After |         LocalBuild | 1000000 |     102.34 ns |   0.125 ns |   0.241 ns |    2 |       - |      - |      - |         - |     288 B |
|    getItem | Before |            Default | 1000000 |     182.97 ns |   0.277 ns |   0.533 ns |    4 |       - |      - |      - |         - |     129 B |
|            |        |                    |         |               |            |            |      |         |        |        |           |           |
|    addItem |  After |         LocalBuild |   10000 |     397.49 ns |   1.525 ns |   3.080 ns |    1 |  0.1094 | 0.0328 |      - |     693 B |    2547 B |
|    addItem | Before |            Default |   10000 |     722.29 ns |   1.054 ns |   2.105 ns |    2 |  0.1104 | 0.0313 |      - |     693 B |     697 B |
|    addItem |  After |         LocalBuild | 1000000 |  73,867.32 ns | 282.677 ns | 571.022 ns |    3 | 16.1000 | 1.1000 | 0.1000 |  100967 B |    2547 B |
|    addItem | Before |            Default | 1000000 | 126,796.51 ns | 386.378 ns | 762.673 ns |    4 | 16.1000 | 1.0000 | 0.1000 |  100967 B |     697 B |
|            |        |                    |         |               |            |            |      |         |        |        |           |           |
| removeItem |  After |         LocalBuild |   10000 |      13.00 ns |   0.050 ns |   0.098 ns |    1 |  0.0064 |      - |      - |      40 B |     495 B |
| removeItem | Before |            Default |   10000 |      16.36 ns |   0.039 ns |   0.079 ns |    2 |  0.0063 |      - |      - |      40 B |     519 B |
| removeItem |  After |         LocalBuild | 1000000 |   1,244.06 ns |   7.826 ns |  14.890 ns |    3 |  0.6000 |      - |      - |    4000 B |     495 B |
| removeItem | Before |            Default | 1000000 |   1,661.32 ns |   9.963 ns |  19.431 ns |    4 |  0.6000 |      - |      - |    4000 B |     519 B |


## MK is not inlined

|     Method |    Job | BuildConfiguration |    Size |          Mean |      Error |     StdDev | Rank |   Gen 0 |  Gen 1 |  Gen 2 | Allocated | Code Size |
|----------- |------- |------------------- |-------- |--------------:|-----------:|-----------:|-----:|--------:|-------:|-------:|----------:|----------:|
|    getItem |  After |         LocalBuild |   10000 |      73.24 ns |   0.245 ns |   0.484 ns |    1 |       - |      - |      - |         - |     288 B |
|    getItem | Before |            Default |   10000 |     127.40 ns |   0.464 ns |   0.883 ns |    3 |       - |      - |      - |         - |     129 B |
|    getItem |  After |         LocalBuild | 1000000 |     103.49 ns |   0.217 ns |   0.407 ns |    2 |       - |      - |      - |         - |     288 B |
|    getItem | Before |            Default | 1000000 |     186.79 ns |   0.370 ns |   0.704 ns |    4 |       - |      - |      - |         - |     129 B |
|            |        |                    |         |               |            |            |      |         |        |        |           |           |
|    addItem |  After |         LocalBuild |   10000 |     399.43 ns |   1.492 ns |   2.910 ns |    1 |  0.1104 | 0.0313 |      - |     693 B |    2547 B |
|    addItem | Before |            Default |   10000 |     741.74 ns |   2.865 ns |   5.654 ns |    2 |  0.1104 | 0.0313 |      - |     693 B |     697 B |
|    addItem |  After |         LocalBuild | 1000000 |  73,717.07 ns | 394.387 ns | 759.850 ns |    3 | 16.1000 | 1.1000 | 0.1000 |  100967 B |    2547 B |
|    addItem | Before |            Default | 1000000 | 128,648.11 ns | 394.757 ns | 769.943 ns |    4 | 16.1000 | 1.1000 | 0.1000 |  100967 B |     697 B |
|            |        |                    |         |               |            |            |      |         |        |        |           |           |
| removeItem |  After |         LocalBuild |   10000 |      13.02 ns |   0.081 ns |   0.154 ns |    1 |  0.0064 |      - |      - |      40 B |     495 B |
| removeItem | Before |            Default |   10000 |      16.59 ns |   0.058 ns |   0.111 ns |    2 |  0.0063 |      - |      - |      40 B |     519 B |
| removeItem |  After |         LocalBuild | 1000000 |   1,363.59 ns |  14.975 ns |  29.207 ns |    3 |  0.6000 |      - |      - |    4000 B |     495 B |
| removeItem | Before |            Default | 1000000 |   1,661.98 ns |  12.086 ns |  23.287 ns |    4 |  0.6000 |      - |      - |    4000 B |     519 B |

## Height is moved to leaves
