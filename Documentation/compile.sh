
shopt -s nullglob
red='\033[0;31m'
NC='\033[0m' # No Color

rm -rf doc/*
rm "IndexList.txt"

cd source
for projDir in *
do
	echo "--Running $projDir--"
	cd ..
	dir=$(echo `pwd` | echo `cut -c 3-`)
	echo c:$dir/doc/$projDir/index.html >> IndexList.txt
	cd source
	
	if Doctran.exe --overwrite -o ../doc/$projDir --project_info $projDir/project.info --save_xml project.xml
	then
		echo "Success"
	else
		echo -e "${red}FALIURE!!!${NC}"
	fi
	echo "--------------------"
done
cd ..