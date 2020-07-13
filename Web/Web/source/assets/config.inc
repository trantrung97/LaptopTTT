<?php
$servername = "localhost"; 
$username = "root";
$password = "";
$dbname = "db_banhang";
$conn = @mysqli_connect($servername,$username,$password,$dbname) or die('Kết nối thất bại:');;
/*if(!$conn){
   die('Kết nối thất bại:');
}else{
    echo"kết nối thành công";
}
mysqli_close($conn);*/
?>
