   M S B A M L   `   `   `  ����  BiomentricoHolding>    8BiomentricoHolding.Views.Empleado.EmpleadoYaExisteDialog   5   	   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	     � Title$  Empleado ya existe��5   	   $	��220��5   	   $	��420��6    � WindowStartupLocation$ CenterScreen=�5   	   
������  � WindowStyle$
 None=�5   	    � 
ResizeMode$ NoResize=�5	   	    � AllowsTransparency$
 True��5
   	    � Topmost$
 True��5   	   .��5      � ���	#80000000.+�5   
   �� 
������� �� CornerRadius$ 16��5       �� Padding$ 25q�5      $	��380��5      $��Center=�5      $��Center=�5         
DropShadow J� Effect# �   5      ._�5      �� .+�5      �� $#��⚠️ Empleado ya registrado��6   $��18(�5      $
��Bold"�5      ���#c70100	 �� TextAlignment$	 Center=�5      $��0,0,0,10q�5      �� 5!      -   5        
 ��Name$
 
txtMensaje��5!      $2��,El empleado ya existe.\n¿Qué deseas hacer?��5"      $��14(�5#      $	 Center=�5$      $
��Wrap=�5%      $��0,0,0,20q�5&      ���#1F29375'      �� 5*       �� Orientation$ 
Horizontal=�6   $��Center=�5+      $��0,10,0,0q�5,      .+�5/      �� -   5         ��Name$ btnModificar��5/      $��✏️ Modificar��50      $	��140��51      $��35��52      $
��Bold"�53      
����������#680307 � Cursor$
 Hand��56      $��0,0,10,0q�58      ��59      �� 5:       �� 
TargetType" ��6$    �� BasedOn# [B��68   .�5;   "   ��  �� Property � �� �� Value 5<   &   �� 5=   *    �� 
TargetType" ��6:   .R�5>   .   �� #��z��65   $ 8��5?   5   $ 10q�5@   5   ._�5A   2   �� $��Center=�6C   $��Center=�5B   C   5C   /   5D   +   5F   #    �� Triggers 5G   "   P� 5H   &    P� Property �@J�IsMouseOver P� Value$
 True��6E   . �5I   *   ��  � �� �#343a406G   5J   '   5L      5N      �� 5Q      -   5        $ 	btnCerrar��5Q      $��
❌ Cerrar��5R      $	��100��5S      $��35��5T      $
��Bold"�5U      ���#E5E7EB���#1F2937$
 Hand��5X      ��5Z      �� 5[      " ��6$   # [B��68   .�5\   "   ��  � �� 5]   &   �� 5^   *   " ��6:   .R�5_   .   �� #��z��65   $ 8��5`   5   $ 10q�5a   5   ._�5b   2   �� $��Center=�6C   $��Center=�5c   C   5d   /   5e   +   5g   #    5h   "   P� 5i   &    �@J�IsMouseOver$
 True��6E   . �5j   *   ��  � �� �#D1D5DB6G   5k   '   5m      5o      5q      5r      5s      5t      5u      