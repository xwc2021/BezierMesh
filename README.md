# BezierMesh
I found UE4 has this feature, so try it by myself.

# 怎麼找出好的help Vector  
一開始想說隨便指定1個help Vecotr
結果生出來的Curve Space並不連續

![image](https://lh3.googleusercontent.com/pw/AM-JKLUaV94aShONmtmYL_FyyX409-3AJCONkH0nL6G6M4yywVbOgFo9mEv3f21IHUDrfOctRaEniimU35ROArBJXERgd3Ll7kQMC0KuqyAPq5Z55_q7sxZZHk6a48ok5BiNvdZCw8w14XcadRAXHIiwY2TAdQ=w341-h398-no?authuser=0)

後來畫著畫著，就發現了這個取法

![image](https://lh3.googleusercontent.com/pw/AM-JKLX3GIDae3M7PkPsFwfYGAx2BR39JQDVsyn2tfcBlp2903ZEVvUXYDm898iEK6UvHyUB7MMaSWdXuUJXj5uJXDZM9fcB4AmMRJT2wZQ9cwAPgeYL-WWnkX5YaPGPfa-Fjlx14ygbQC3d0ctrb37EUXnKwg=w571-h507-no?authuser=0)

![image](https://lh3.googleusercontent.com/pw/AM-JKLUssN9Ub_rJPDlSGczW8W8_kKVmVSdjuep62rkuxIZcU0o7M_IY6G8EGbfJMz2Gp6S6KE1fdplueyriXRz1k4RvpJDTEVGGgnGZDmKWGCpvUrUO2hReF5kXxYxQJ1l6tQlSJMJ6ejE55Xwy2hHMfbWU5w=w630-h423-no?authuser=0)

# Mesh along Curve
令 Bezier Curve的tangent 為Y軸  
用help Vector和Y軸，生出X軸、Z軸  
這樣就可以把mesh的vertex從Local space變換到Curve Space了  

![image](https://lh3.googleusercontent.com/pw/AM-JKLWxY4MLCsF9mMvbpfHrUIEiiRivXSMnc9DCZ2WElT61LBZaLAVKXTjmLDMwsEnlHb2QX-bOQjyO-WEpSekWgG6-ZlUpz4gPeTp9uwoj3cM1mcVnmS7PTNj0rPTj1_Ly9DmzYYI9I8iaGTaxvLlwCryY-w=w1615-h935-no?authuser=0)
