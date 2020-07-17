call envset.bat

cd tfpose
python run.py  --image=./input.png  --model=%1
