 
..\TopShelf_WindowsService.exe install
Net Start TopShelfService
sc config TopShelfService start= delayed-auto
pause