########### Python 2.7 #############
## Sample script for calling the moderator EvaluateImage Api with a local file
## To run this script, python 2.7 is required.
####################################
import httplib, urllib, base64

headers = {
    # Request headers
    # Replace the placeholder {Add your Subscription Id} to add your subscription id for this Api, accessible in your profile:
    # https://developer.microsoftmoderator.com/developer
    'Content-Type': 'image/jpeg',
    'Ocp-Apim-Subscription-Key': '{Add your Subscription Id}',
}

params = urllib.urlencode({
})

try:

    ## Add the file name to a local image
    ## The content type is set for jpeg images. If you provide a different image type
    ## Update the local type
    filename = '{Add the path to a local file}'

    conn = httplib.HTTPSConnection('api.microsoftmoderator.com')
    conn.request("POST", "/Image/v2/Evaluate", open(filename, "rb"), headers)
    response = conn.getresponse()
    data = response.read()
    print(data)
    conn.close()
except Exception as e:
    print("[Error {0}] {1}".format(e.args, e.message))

####################################

