   M S B A M L   `   `   `  ����   BiomentricoHoldingB    <BiomentricoHolding.Views.Empleado.CapturaEntradaSalidaWindow   W SPresentationCore, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=8.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=8.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml       � Title$"  Registro de Entrada / Salida�� � WindowStartupLocation$ CenterScreen=�$	��720��$	��500�����#f8f9fa � 
ResizeMode$ NoResize=�.��� $��20q� � RowDefinitions �� $
��Auto ��� $
��Auto ��� $��* ��� $
��Auto �.+��� $��0�� �� Orientation$ 
Horizontal=�$��Center=�$��0,0,0,10q�.+��� $(��"/Recursos/icono_entrada_salida.png��$��32��$��32��$��0,0,10,0q��� $"��Registro de Entrada y Salida��$��24(�$
��Bold"����#212529$��Center=��� $��1��$��Center=�$��Center=�$��	0,10,0,10q�.+��� -    ��Name$ txtReloj��$��36(�$
��Bold"����#c70100$��Center=��� -   $ txtFecha��$��16(�
�������$��Center=�$��0,4,0,0q�� $��2��$��Center=�$��Center=� � ColumnDefinitions �� $��3* ��� $��2* �.+��� $��0��$	��400��$��0,0,20,0q�.+��� -   $ txtEstadoHuella��$,��&Por favor coloque su dedo en el lector��$��18(� �� TextAlignment$ Center=�
�������$
��0,10q��� 
����������#dee2e6$��1q� �� CornerRadius$ 8��	 �� Padding$	 20q�$
��0,10q�._��� .+��� -   $ lblNombreEmpleado��$��Nombre: ---��$��16(�$	��0,5q��� -   $ lblDocumento��$��Documento: ---��$��16(�$	��0,5q��� -   $ lblTipoMarcacion��$��Marcación: ---��$��16(�$	��0,5q��� -   $ lblEstadoMarcacion��$��Estado: ---��$��16(�$	��0,5q�
��� d ��� $��1��$	��200��$	��240��
����������#dee2e6$��1q�$ 8��$	 10q�$��Center=�._��� -   
 ��Name$
 	imgHuella��$	��180��$	��220��$��Uniform=�$��Center=�$��Center=��� -	   $��3��$��Reiniciar Captura��$��Center=�$
��0,10q�$
��12,6q����#c70100
�������$
��Bold"�$	��200��$��0q� � Cursor$
 Hand������  �� 
TargetType" ��.R��� #��z��$ 6��#	 z��._��� $��Center=�$��Center=�$
��Bold"�