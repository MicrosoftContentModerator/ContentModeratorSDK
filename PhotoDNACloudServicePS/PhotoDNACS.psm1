#Copyright (c) 2015 Microsoft Corporation 

#Permission is hereby granted, free of charge, to any person obtaining a copy
#of this software and associated documentation files (the "Software"), to deal
#in the Software without restriction, including without limitation the rights
#to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
#copies of the Software, and to permit persons to whom the Software is
#furnished to do so, subject to the following conditions:

#The above copyright notice and this permission notice shall be included in
#all copies or substantial portions of the Software.

#THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
#IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
#FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
#AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
#LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
#OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
#THE SOFTWARE.

Function Get-FilePhotoDNA
{

    <#
        .SYNOPSIS
         Submit an individual image file to Microsoft PhotoDNA Cloud Service 

        .DESCRIPTION
         Reads the extension & contents of the target file and submits an HTTP POST request to Microsoft PhotoDNA Cloud Service.

        .LINK
         Get-DirectoryPhotoDNA

        .PARAMETER APIKey
         The subscription key from https://myphotodna.microsoftmoderator.com

        .PARAMETER TargetFile
         A file of a supported image type, currently BMP, JPEG, GIF, PNG, & TIFF

        .PARAMETER Uri
         The region based uri from https://myphotodna.microsoftmoderator.com

        .EXAMPLE
         Get-FilePhotoDNA -APIKey <Your PhotoDNA Subscription key> -TargetFile <Absolute Path to the Image File> -Uri <PhotoDNA Match endpoint> 
         This command returns the service JSON response as an object.
    #>

    [CmdletBinding(PositionalBinding=$false)]

    param (
        [Parameter(Mandatory=$true)]
        [string]$APIKey,
        
        [Parameter(Mandatory=$true)]
        [string]$TargetFile,

        [Parameter(Mandatory=$true)]
        [string]$Uri
    )

    Try{
        $Subject = (Get-Item $TargetFile -EA Stop)
        
        switch($Subject.Extension) {
            {@('.jpeg', '.jpg') -contains $_} {$ContentType = "image/jpeg"}
            {@('.gif') -contains $_} {$ContentType = "image/gif"}
            {@('.png') -contains $_} {$ContentType = "image/png"}
            {@('.bmp') -contains $_} {$ContentType = "image/bmp"}
            {@('.tiff', '.tif') -contains $_} {$ContentType = "image/tiff"}
        }

        if((Get-Item $TargetFile).length -eq 0){


           Write-Error "Submitted file is empty"
       
        }
        else{
           return (Invoke-RestMethod -Uri $Uri -ContentType $ContentType -Headers @{"Ocp-Apim-Subscription-Key" = $APIKey} -Method POST -InFile $Subject.FullName)
        }
    }
    Catch{
       $ErrorMessage = $_.Exception.Message

       Write-Error "$ErrorMessage"
    }
}

Function Get-DirectoryPhotoDNA
{

    <#
        .SYNOPSIS
         Submit all new image files in a directory to Microsoft PhotoDNA Cloud Service  

        .DESCRIPTION
         Recursively checks a directory for image files added, created, or modified since the previous run (or all image files on first run) and writes successful results to a CSV output file in the target directory.

        .LINK
         Get-FilePhotoDNA

        .PARAMETER APIKey
         The subscription key from https://developer-westus.microsoftmoderator.com/developer; may require logging in through Azure portal to access.

        .PARAMETER TargetDirectory
         Specify the root directory to screen for new images; defaults to current directory. Output file will be written to this directory as well

        .PARAMETER Uri
        The region based uri from https://myphotodna.microsoftmoderator.com

        .EXAMPLE
         Get-DirectoryPhotoDNA -APIKey key -TargetDirectory directory -Uri <PhotoDNA Match endpoint> 
         This command writes (on first run) or updates (on subsequent runs) the output file in CSV format to the target directory for all successful requests to Microsoft PhotoDNA Cloud Service.
    #>

    [CmdletBinding(PositionalBinding=$false)]

    param (
        [Parameter(Mandatory=$true)]
        [string]$APIKey,
        
        [Parameter(Mandatory=$false)]
        [string]$TargetDirectory = '.',

        [Parameter(Mandatory=$true)]
        [string]$Uri
    )

    $SupportedExtensions = @('.jpeg', '.jpg', '.gif', '.png', '.bmp', '.tif', '.tiff')
    $datestamp = (Get-Date).ToString("s").Replace(":","-") 
    $LogFile = Join-Path -Path $TargetDirectory -ChildPath "PhotoDNA Results$datestamp.csv"
    $ThisCheck = (Get-Date).ToUniversalTime()

    $LastCheck = '1900-01-01 00:00:00'

    $PreviouslyChecked = @()

    if(Test-Path $LogFile) {
        Import-Csv $LogFile |
            ForEach-Object {
                $LastCheck = $_."Check Time"
                if($_."Status Code" -eq 3000) {
                    $PreviouslyChecked += $_."File Name"
                }
            }
    }
    else {
        Add-Content -Path $LogFile -Value "Check Time,File Name,Tracking ID,Status Code,Status Description,Is Match,Match ID,Match Source,Match Severity"
    }
    
    $Then = [datetime]::Parse($LastCheck)

    Get-ChildItem -Recurse $TargetDirectory |
    ForEach-Object {
        if ($SupportedExtensions -contains $_.Extension -and ($_.LastWriteTimeUtc -ge $Then -or $PreviouslyChecked -notcontains $_.FullName)) {
            $response = (Get-FilePhotoDNA -APIKey $APIKey -TargetFile $_.FullName -Uri $Uri )
            Add-Content -Path $LogFile -Value "$($ThisCheck),$($_.FullName),$($response.TrackingId),$($response.Status.Code),$($response.Status.Description),$($response.IsMatch),$($response.MatchDetails.MatchFlags.AdvancedInfo.Value),$($response.MatchDetails.MatchFlags.Source),$($response.MatchDetails.MatchFlags.Violations)";
            if($response.Status.Code -ne 3000) {
                Write-Warning "Did not process $($_.FullName)"
            }
        }
    }
}
