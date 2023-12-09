<food>

	<Recipes>
	
		<!-- 需要加热    -->
		<Recipe Result="FCORFoodBlock:10" ResultCount="1" Remains="EmptyBucketBlock" RemainsCount="1" RequiredHeatLevel="1" a="rawfish" b="waterbucket" Description="制作清蒸鱼">
		  "ab"
		</Recipe>
		<Recipe Result="Chocolate" ResultCount="1" RequiredHeatLevel="1" a="fcfood:25"  Description="生巧克力做巧克力">
		  "a"
		</Recipe>
		
		<Recipe Result="FCORFoodBlock:15" ResultCount="2"  RequiredHeatLevel="1" a="fcfood:25" Description="水瓶蒸馏制作盐">
		  "a"
		</Recipe>
		<Recipe Result="FCORFoodBlock:4" ResultCount="1" RequiredHeatLevel="1" a="fcfood:11" Description="生瓜子烤制瓜子">
		  "a"
		</Recipe>
		<Recipe Result="FCORFoodBlock:23" ResultCount="5"  RequiredHeatLevel="1" a="fcfood:12" b="fcfood:22" Description="生鸡块和油瓶炸成炸鸡">
		  "ab"
		</Recipe>
		<Recipe Result="FCORFoodBlock:18" ResultCount="5" RequiredHeatLevel="1" a="fcfood:14" b="fcfood:22" Description="小酥肉，由油和肉片炸制">
		  "ab"
		</Recipe>
		
		
		<!-- 高炉区   <Recipe Result="FCItemBlock:4" ResultCount="1" RequiredHeatLevel="2" a="sand" b="fcitem:16" Description="粗硅">
		  "ab"
		</Recipe>
		<Recipe Result="GlassBlock" ResultCount="10" RequiredHeatLevel="2" a="sand" b="cobblestone" Description="快速烧玻璃">
		  "ab"
		</Recipe>
		<Recipe Result="FCItemBlock:15" ResultCount="2" RequiredHeatLevel="2" a="ironingot" b="fcitem:16"  Description="钢锭">
		  "ab"
		</Recipe>
		<Recipe Result="GraniteBlock" ResultCount="2" RequiredHeatLevel="2" a="cobblestone" Description="[0]">
		  "aa"
		</Recipe>
		<Recipe Result="IronIngotBlock" ResultCount="2" RequiredHeatLevel="2" a="ironorechunk" Description="[0]">
		  "aa"
		</Recipe>
		<Recipe Result="CopperIngotBlock" ResultCount="2" RequiredHeatLevel="2" a="malachitechunk" Description="[0]">
		  "aa"
		</Recipe>
		<Recipe Result="GermaniumChunkBlock" ResultCount="2" RequiredHeatLevel="2" a="germaniumorechunk" Description="[0]">
		  "aa"
		</Recipe>
		<Recipe Result="BrickBlock" ResultCount="2" RequiredHeatLevel="2" a="clay" Description="[0]">
		  "aa"
		</Recipe> -->
		
		
		<!-- 机床区    -->
		
		<Recipe Result="FCItemBlock:6" ResultCount="1" RequiredHeatLevel="10" a="semiconductorblock" b="fcitem:5" c="fcitem:0" d="fcitem:8" e="fcitem:3" Description="集成芯片需要在机床上制作">
		  "aba"
		  "ece"
		  "ada"
		</Recipe>
		<Recipe Result="FCItemBlock:10" ResultCount="1" RequiredHeatLevel="10" a="diamond" b="fcitem:6" c="fcitem:2" d="fcitem:5" e="andgate" f="orgate" g="fcitem:8" Description="偏导芯片需要在机床上制作">
		  "aca"
		  "dbd"
		  "egf"
		</Recipe>
		<Recipe Result="FCMicro" ResultCount="1" RequiredHeatLevel="10" a="glass" b="fcitem:10" c="semiconductorblock" d="fcitem:2"  Description="微观提取器（超级科技方块）需要在机床上制作">
		  "aaa"
		  "dbd"
		  "dcd"
		</Recipe>
		<Recipe Result="FCFermentation" ResultCount="1" RequiredHeatLevel="10" a="glass" b="fcitem:10" c="fcitem:5" d="fcitem:2"  Description="硝石转换器（超级科技方块）需要在机床上制作">
		  "cac"
		  "dbd"
		  "ddd"
		</Recipe>
		
		<!-- 基础工业合成区    -->
		<Recipe Result="FCItemBlock:9" ResultCount="1" RequiredHeatLevel="0" a="stonechunk" b="flour"  Description="磨成精制淀粉">
		  "ab"
		</Recipe>
		<Recipe Result="FCItemBlock:7" ResultCount="1" RequiredHeatLevel="0" a="fcitem:9" b="fcitem:13"  Description="合成酵素粉">
		  "ab"
		</Recipe>
		
		<Recipe Result="FCItemBlock:3" ResultCount="1" RequiredHeatLevel="0" a="wire"  Description="铜线圈">
		  "aaa"
		  "aaa"
		  "aaa"
		</Recipe>
		<Recipe Result="FCItemBlock:0" ResultCount="1" RequiredHeatLevel="0" a="copperingot"  Description="铜板">
		  "aaa"
		</Recipe>
		<Recipe Result="FCItemBlock:1" ResultCount="1" RequiredHeatLevel="0" a="ironingot"  Description="铁板">
		  "aaa"
		</Recipe>
		<Recipe Result="FCItemBlock:8" ResultCount="1" RequiredHeatLevel="0" a="fcitem:4"  Description="硅板">
		  "aaa"
		</Recipe>
		<Recipe Result="FCItemBlock:2" ResultCount="1" RequiredHeatLevel="0" a="fcitem:15"  Description="钢板">
		  "aaa"
		</Recipe>
		<Recipe Result="FCItemBlock:17" ResultCount="3" RequiredHeatLevel="0" a="ironingot"  Description="铁粉">
		  "a"
		</Recipe>
		<Recipe Result="FCItemBlock:16" ResultCount="3" RequiredHeatLevel="0" a="coalchunk"  Description="炭粉">
		  "a"
		</Recipe>
		<Recipe Result="FCItemBlock:18" ResultCount="1" RequiredHeatLevel="0" a="rod" b="fcitem:17" c="cottonwad" Description="磁化铁棒">
		  "  b"
		  " a "
		  "c  "
		</Recipe>
		<Recipe Result="FCItemBlock:11" ResultCount="1" RequiredHeatLevel="0" a="rod"  Description="齿轮制作">
		  " a "
		  "a a"
		  " a "
		</Recipe>
		<Recipe Result="FCItemBlock:12" ResultCount="1" RequiredHeatLevel="0" a="fcitem:15"  Description="钢棒制作">
		  " a "
		  " a "
		</Recipe>
		<Recipe Result="FCItemBlock:19" ResultCount="1" RequiredHeatLevel="0" a="rod" b="fcitem:18" c="fcitem:3" d="fcitem:1" Description="马达制作">
		  "dca"
		  "cbc"  
		  "acd"
		</Recipe>
		<Recipe Result="FCItemBlock:14" ResultCount="1" RequiredHeatLevel="0" a="rod" b="fcitem:19" c="fcitem:3" d="fcitem:1" e="fcitem:11" Description="活塞制作">
		  "ddd"
		  "caa"
		  "cbe"
		</Recipe>
		<Recipe Result="FCItemBlock:5" ResultCount="1" RequiredHeatLevel="0" a="fcitem:0" b="fcitem:1" c="fcitem:3" d="fcitem:8" Description="线路板制作">
		  "cac"
		  "cbc"
		  "cdc"
		</Recipe>
		
		<Recipe Result="JichuangBlock" ResultCount="1" RequiredHeatLevel="0" a="fcitem:5" b="fcitem:14" c="fcitem:3" d="fcitem:2"  Description="机床制作">
		  "ddd"
		  "aba"
		  "cbc"
		</Recipe>
		<Recipe Result="GaoluBlock" ResultCount="1" RequiredHeatLevel="0" a="fcitem:1" b="fcitem:3"  Description="高炉制作">
		  "aaa"
		  "a a"
		  "aba"
		</Recipe>
		<Recipe Result="FajiaotongBlock" ResultCount="1" RequiredHeatLevel="0" a="fcitem:1" b="fcitem:3" c="fcitem:6" Description="发酵桶制作">
		  "aaa"
		  "aca"
		  "aba"
		</Recipe>
		
		
		
		
		<Recipe Result="FCORFoodBlock:8" ResultCount="2" RequiredHeatLevel="0" a="bread" Description="面包片">
		  "a" 
		</Recipe>
		<Recipe Result="FCORFoodBlock:16" ResultCount="1" RequiredHeatLevel="0" a="kelp" b="dough" Description="寿司">
		  "aaa"
		  "aba"
		  "aaa"
		</Recipe>
		<Recipe Result="FCORFoodBlock:9" ResultCount="1" RequiredHeatLevel="0" a="tallgrass" b="dough" Description="青团">
		  "aaa"
		  "aba"
		  "aaa"
		</Recipe>
		<Recipe Result="FCORFoodBlock:0" ResultCount="1" Remains="EmptyBucketBlock" RemainsCount="1" RequiredHeatLevel="0" a="seeds:5" b="dough" c="milkbucket" d="fcfood:22" Description="[0]">
		  "aaa"
		  "cbd"
		  "aaa"
		</Recipe>
		<Recipe Result="FCORFoodBlock:11" ResultCount="2" RequiredHeatLevel="0" a="sunflower" Description="生瓜子">
		  "a" 
		</Recipe>
		<Recipe Result="FCSeedBlock:1" ResultCount="18" RequiredHeatLevel="0" a="sunflower" Description="向日葵种子">
		  "aaa" 
		  "aaa" 
		  "aaa" 
		</Recipe>
		<Recipe Result="FCORFoodBlock:24" ResultCount="2" RequiredHeatLevel="0" a="xigua:8" Description="西瓜片">
		  "a" 
		</Recipe>
		
		<Recipe Result="FCORFoodBlock:17" ResultCount="1" RequiredHeatLevel="0" a="tallgrass" b="dough" c="fcfood:15" d="cookedmeat" Description="馅饼">
		  "ac"
		  "db"
		</Recipe>
		<Recipe Result="FCORFoodBlock:6" ResultCount="1" RequiredHeatLevel="0" a="fcfood:7" b="cookedbird" c="fcfood:15" Description="黄焖鸡=酱油+熟鸡+盐">
		  "abc"
		</Recipe>
		<Recipe Result="FCORFoodBlock:3" ResultCount="1" RequiredHeatLevel="0" a="tallgrass" Description="葱">
		  "aaa"
		  "aaa"
		  "aaa"
		</Recipe>
		<Recipe Result="FCORFoodBlock:5" ResultCount="1" RequiredHeatLevel="0" a="fcfood:8" b="cookedmeat" c="fcfood:3" d="fcfood:22" Description="汉堡=面包片+熟肉+葱+油瓶">
		  " a "
		  "cbd"
		  " a "
		</Recipe>
		<Recipe Result="FCORFoodBlock:12" ResultCount="2" RequiredHeatLevel="0" a="rawbird"  Description="鸟肉可以分解成生鸡块">
		  "aa" 
		</Recipe>
		<Recipe Result="FCORFoodBlock:14" ResultCount="1" RequiredHeatLevel="0" a="flour" b="cookedmeat" c="fcfood:15" Description="待处理的肉片=面粉+熟肉+盐">
		  "abc"
		</Recipe>
		
		
		
		
		
		
		
		
		
		
		
		<!-- 发酵桶合成区    -->
		<Recipe Result="Jiutong" ResultCount="1" RequiredHeatLevel="1000" a="rotjiutong" Description="用发酵桶发酵酒桶">
		  "a"
		</Recipe>
		<Recipe Result="YHjiutong" ResultCount="1" RequiredHeatLevel="1000" a="rawyhjiutong" Description="用发酵桶发酵樱花酒桶">
		  "a"
		</Recipe>
		<Recipe Result="Jiangyoutong" ResultCount="1" RequiredHeatLevel="1000" a="youtong" Description="用发酵桶发酵酱油">
		  "a"
		</Recipe>
		
		
		
		
	    <!-- 物品类合成区    -->
		
		<Recipe Result="EmptyFCBucketBlock" ResultCount="1" RequiredHeatLevel="0" a="glass"   Description="空玻璃杯">
		  "a a"
		  "a a"
		  "aaa"
		</Recipe>
		<Recipe Result="FCORFoodBlock:2" ResultCount="9" RequiredHeatLevel="0" a="glass" b="planks"   Description="制作一次性玻璃瓶">
		  " b "
		  "a a"
		  " a "
		</Recipe>
		
		
		
		
		
		
		
		
	    <!-- 1.0.9果汁食物类合成区<Recipe Result="Xiguazhi" ResultCount="1"  RequiredHeatLevel="0" a="xigua:24" b="boliping2" Description="西瓜汁2">
		  "ab"
		</Recipe>    -->
		<Recipe Result="Xiguazhi" ResultCount="1"  RequiredHeatLevel="0" a="xigua:8" b="boliping2" Description="西瓜汁">
		  "ab"
		</Recipe>
		
		<Recipe Result="Pingguozhi" ResultCount="1"  RequiredHeatLevel="0" a="pingguo" b="boliping2" Description="[0]">
		  "aa"
		  " b "
		</Recipe>
		<Recipe Result="Juzizhi" ResultCount="1"  RequiredHeatLevel="0" a="juzi" b="boliping2" Description="[0]">
		  "aa"
		  " b "
		</Recipe>
		<Recipe Result="Cafe" ResultCount="1" Remains="EmptyBucketBlock" RemainsCount="1" RequiredHeatLevel="0" a="cocofeng" b="boliping2" c="waterbucket"  Description="[0]">
		  "aaa"
		  " bc"
		</Recipe>
		<Recipe Result="Cocofeng" ResultCount="3"  RequiredHeatLevel="0" a="cocobean" Description="磨成可可粉">
		  "a"
		</Recipe>
		<Recipe Result="FCORFoodBlock:25" ResultCount="1" Remains="EmptyBucketBlock" RemainsCount="1" RequiredHeatLevel="0" a="cocofeng"  b="milkbucket" Description="[0]">
		  "aaa"
		  "aba"
		  "aaa"
		</Recipe>
		
		
		
		
		
		<!-- 樱花木合成区    -->
		<Recipe Result="YHPlanksBlock" ResultCount="4" RequiredHeatLevel="0" a="yhwood" Description="樱花木板">
		  "a"
		</Recipe>
		<Recipe Result="WoodenDoorBlock" ResultCount="1" RequiredHeatLevel="0" a="yhplanks" Description="[0]">
			"aa"
			"aa"
			"aa"
		</Recipe>
		<Recipe Result="PlanksBlock" ResultCount="10" RequiredHeatLevel="0" a="ljwood" Description="老君木可以提供更多木板">
		  "a"
		</Recipe>
		<Recipe Result="StickBlock" ResultCount="4" RequiredHeatLevel="0" a="yhplanks" Description="[0]">
		  "a"
		  "a"
		</Recipe>
		<Recipe Result="CraftingTableBlock" ResultCount="1" RequiredHeatLevel="0" a="yhplanks" Description="[0]">
		  "aa"
		  "aa"
		</Recipe>
		<Recipe Result="YHcarpetBlock" ResultCount="1" RequiredHeatLevel="0" a="yh" Description="合成樱花地毯">
		  "aa"
		  "aa"
		</Recipe>
	
	
	
	
		<!-- 树苗区    -->
		<Recipe Result="FCSaplingBlock" ResultCount="1" RequiredHeatLevel="0" a="cocoleaves" Description="可可树苗">
          "a"
		</Recipe>
		<Recipe Result="FCSaplingBlock:1" ResultCount="1" RequiredHeatLevel="0" a="orangeleaves" Description="[0]">
		  "a"
		</Recipe>
		<Recipe Result="FCSaplingBlock:2" ResultCount="1" RequiredHeatLevel="0" a="appleleaves" Description="[0]">
		  "a"
		</Recipe>
		<Recipe Result="FCSaplingBlock:3" ResultCount="1" RequiredHeatLevel="0" a="yinghualeaves" Description="[0]">
		  "a"
		</Recipe>
		<Recipe Result="FCSaplingBlock:4" ResultCount="1" RequiredHeatLevel="0" a="lorejunleaves" Description="[0]">
		  "a"
		</Recipe>
		
		
		
		
		<!-- 各种液体桶的衍生物区    -->
		<Recipe Result="FCORFoodBlock:7" ResultCount="9" Remains="EmptyBucketBlock" RemainsCount="1" RequiredHeatLevel="0" a="fcfood:2" b="jiangyoutong" Description="酱油">
		  "aaa"
		  "aba"
		  "aaa"
		</Recipe>
		<Recipe Result="FCORFoodBlock:1" ResultCount="9" Remains="EmptyBucketBlock" RemainsCount="1" RequiredHeatLevel="0" a="fcfood:2" b="jiutong"  Description="啤酒">
		  "aaa"
		  "aba"
		  "aaa"
		</Recipe>
		<Recipe Result="YHBeer" ResultCount="9" Remains="EmptyBucketBlock" RemainsCount="1" RequiredHeatLevel="0" a="fcfood:2" b="yhjiutong"  Description="啤酒">
		  "aaa"
		  "aba"
		  "aaa"
		</Recipe>
		<Recipe Result="FCORFoodBlock:26" ResultCount="9" Remains="EmptyBucketBlock" RemainsCount="1" RequiredHeatLevel="0" a="fcfood:2" b="waterbucket" Description="[0]">
		  "aaa"
		  "aba"
		  "aaa"
		</Recipe>
		<Recipe Result="FCORFoodBlock:22" ResultCount="9" Remains="EmptyBucketBlock" RemainsCount="1" RequiredHeatLevel="0" a="fcfood:2" b="youtong" Description="[0]">
		  "aaa"
		  "aba"
		  "aaa"
		</Recipe>
		
		
		
		
		
		<!-- 各种液体桶区    -->
		<Recipe Result="Rotjiutong" ResultCount="1"  RequiredHeatLevel="0" a="seeds:5" b="waterbucket"  Description="未发酵的酒桶">
		  "aaa"
		  "aba"
		  "aaa"
		</Recipe>
		<Recipe Result="Youtong" ResultCount="1" RequiredHeatLevel="0" a="fcfood:11" b="emptybucket" Description="油桶">
		  "aaa"
		  "aba"
		  "aaa"
		</Recipe>
		<Recipe Result="Youtong" ResultCount="1" RequiredHeatLevel="0" a="fcfood:3" b="emptybucket" Description="油桶（葱）">
		  "aaa"
		  "aba"
		  "aaa"
		</Recipe>
		
		
		<Recipe Result="RotYHjiutong" ResultCount="1"  RequiredHeatLevel="0" a="yh" b="rotjiutong"  Description="未发酵的樱花酒">
		  "aaa"
		  "aba"
		  "aaa"
		</Recipe>
		
		
		
		<!-- 腌肉区    -->
		<Recipe Result="FCORFoodBlock:21" ResultCount="1" RequiredHeatLevel="0" a="rawmeat" b="fcfood:15" Description="腌肉">
		  "ab" 
		</Recipe>
		<Recipe Result="FCORFoodBlock:19" ResultCount="1" RequiredHeatLevel="0" a="rawbird" b="fcfood:15" Description="腌鸡">
		  "ab" 
		</Recipe>
		<Recipe Result="FCORFoodBlock:20" ResultCount="1" RequiredHeatLevel="0" a="rawfish" b="fcfood:15" Description="腌鱼">
		  "ab" 
		</Recipe>
		
		
		<!-- 咸蛋区    -->
		<Recipe Result="FCORFoodBlock:13" ResultCount="1" RequiredHeatLevel="0" a="egg:0" b="fcfood:15" Description="生咸蛋">
		  "ab" 
		</Recipe>
		<Recipe Result="FCORFoodBlock:13" ResultCount="1" RequiredHeatLevel="0" a="egg:16" b="fcfood:15" Description="[0]">
		  "ab" 
		</Recipe>
		<Recipe Result="FCORFoodBlock:13" ResultCount="1" RequiredHeatLevel="0" a="egg:32" b="fcfood:15" Description="[0]">
		  "ab" 
		</Recipe>
		<Recipe Result="FCORFoodBlock:13" ResultCount="1" RequiredHeatLevel="0" a="egg:768" b="fcfood:15" Description="[0]">
		  "ab" 
		</Recipe>
		
		<Recipe Result="FCORFoodBlock:13" ResultCount="1" RequiredHeatLevel="0" a="egg:784" b="fcfood:15" Description="[0]">
		  "ab" 
		</Recipe>
		
		
		
		
		
		
		
		
		<Recipe Result="LightStoneBlock" ResultCount="1" RequiredHeatLevel="0" a="lightdust" Description="将萤石粉末组合成萤石块">
		  "aa"
		  "aa"
		</Recipe>
		<Recipe Result="LightDustBlock" ResultCount="4" RequiredHeatLevel="0" a="lightstone" Description="萤石块分解">
		  "a"
			
		</Recipe>
		<Recipe Result="FCSeedBlock:0" ResultCount="4" RequiredHeatLevel="0" a="fcfood:24" Description="西瓜片提取西瓜种子">
		  "a"
		</Recipe>
        
		
		
    </Recipes>
	
</food>