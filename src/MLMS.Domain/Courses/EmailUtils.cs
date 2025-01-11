namespace MLMS.Domain.Courses;

public static class EmailUtils
{
    public static string GetAssignmentEmailBody(string courseName) =>
        $"""
        <!DOCTYPE html>
        <html lang="en"><head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <link rel="preconnect" href="https://fonts.googleapis.com">
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="">
            <link href="https://fonts.googleapis.com/css2?family=Sora:wght@100..800&amp;display=swap" rel="stylesheet">
            <title>Makassed LMS</title>
        </head>
        
        <body style="margin:0; padding:0; font-family: Sora, sans-serif; background-color: #f4f4f4;">
            <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="background-color: #f4f4f4; padding: 20px;">
                <tbody><tr>
                    <td align="center">
                        <!-- Main Container -->
                    </td><td style="padding-top:0;padding-bottom:0;padding-right:10px;padding-left:10px">
                        <div class="m_-533078734955811462webkit" style="max-width:600px;margin-top:0;margin-bottom:0;margin-right:auto;margin-left:auto">
        
                            <table role="presentation" class="m_-533078734955811462outer" align="center" style="border-spacing:0;Margin:0 auto;width:100%;max-width:600px">
                                <tbody>
        
                                    <tr>
                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                    </tr>
        
                                    <tr>
                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                    </tr>
        
                                    <tr>
                                        <td class="m_-533078734955811462one-column" align="center" style="padding-left:10px;padding-right:10px">
        
        
                                            <table role="presentation" align="center" border="0" cellpadding="0" cellspacing="0" width="100%" style="border-radius:4px;background-color:#efebe4" bgcolor="#efebe4">
                                                <tbody>
        
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                    </tr>
        
                                                    <tr>
                                                        <td align="center" style="padding-top:0px;padding-left:20px;padding-right:20px">
                                                            <h2 style="color:#027e7b;font-size:28px;font-weight:bold;font-stretch:normal;font-style:normal;line-height:36px;letter-spacing:-0.5px">A Course Has Been Assigned to You!</h2>
                                                        </td>
                                                    </tr>
        
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                    </tr>
        
        
                                                    <tr>
                                                        <td width="0" height="22" style="font-size:1px;line-height:1px" bgcolor="#ffffff"></td>
                                                    </tr>
        
                                                    <tr>
                                                        <td align="left" style="padding-top:0px;padding-left:20px;padding-bottom:0px;padding-right:20px" bgcolor="#ffffff">
                                                            <p style="color:#05192d;font-size:16px;line-height:26px;font-weight:normal">The course {courseName} has been assigned to you you can check it from your LMS Dashboard. Happy Learning :)</p>
                                                        </td>
                                                    </tr>
        
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px" bgcolor="#ffffff"></td>
                                                    </tr>
        
                                                    
        
                                                    
        
        
        
                                                </tbody>
                                            </table>
        
                                        </td>
                                    </tr>
        
        
                                </tbody>
                            </table>
        
                        </div>
                    </td>
                    
                </tr>
            </tbody></table>
        
        
        </body></html>
        """;
    
    public static string GetUnassignmentEmailBody(string courseName) =>
        $"""
        <!DOCTYPE html>
        <html lang="en"><head>
            <meta charset="UTF-8">
            <meta name="viewport" content="width=device-width, initial-scale=1.0">
            <link rel="preconnect" href="https://fonts.googleapis.com">
            <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="">
            <link href="https://fonts.googleapis.com/css2?family=Sora:wght@100..800&amp;display=swap" rel="stylesheet">
            <title>Makassed LMS</title>
        </head>
        
        <body style="margin:0; padding:0; font-family: Sora, sans-serif; background-color: #f4f4f4;">
            <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="background-color: #f4f4f4; padding: 20px;">
                <tbody><tr>
                    <td align="center">
                        <!-- Main Container -->
                    </td><td style="padding-top:0;padding-bottom:0;padding-right:10px;padding-left:10px">
                        <div class="m_-533078734955811462webkit" style="max-width:600px;margin-top:0;margin-bottom:0;margin-right:auto;margin-left:auto">
        
                            <table role="presentation" class="m_-533078734955811462outer" align="center" style="border-spacing:0;Margin:0 auto;width:100%;max-width:600px">
                                <tbody>
        
                                    <tr>
                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                    </tr>
        
                                    <tr>
                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                    </tr>
        
                                    <tr>
                                        <td class="m_-533078734955811462one-column" align="center" style="padding-left:10px;padding-right:10px">
        
        
                                            <table role="presentation" align="center" border="0" cellpadding="0" cellspacing="0" width="100%" style="border-radius:4px;background-color:#efebe4" bgcolor="#efebe4">
                                                <tbody>
        
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                    </tr>
        
                                                    <tr>
                                                        <td align="center" style="padding-top:0px;padding-left:20px;padding-right:20px">
                                                            <h2 style="color:#027e7b;font-size:28px;font-weight:bold;font-stretch:normal;font-style:normal;line-height:36px;letter-spacing:-0.5px">A Course Has Been Unassigned From You!</h2>
                                                        </td>
                                                    </tr>
        
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                    </tr>
        
        
                                                    <tr>
                                                        <td width="0" height="22" style="font-size:1px;line-height:1px" bgcolor="#ffffff"></td>
                                                    </tr>
        
                                                    <tr>
                                                        <td align="left" style="padding-top:0px;padding-left:20px;padding-bottom:0px;padding-right:20px" bgcolor="#ffffff">
                                                            <p style="color:#05192d;font-size:16px;line-height:26px;font-weight:normal">The course {courseName} has been unassigned from you. Don't Forget Other Courses!</p>
                                                        </td>
                                                    </tr>
        
                                                    <tr>
                                                        <td width="0" height="32" style="font-size:1px;line-height:1px" bgcolor="#ffffff"></td>
                                                    </tr>
        
                                                    
        
                                                    
        
        
        
                                                </tbody>
                                            </table>
        
                                        </td>
                                    </tr>
        
        
                                </tbody>
                            </table>
        
                        </div>
                    </td>
                    
                </tr>
            </tbody></table>
        
        
        </body></html>
        """;

    public static string GetCourseExpiredEmailBody(string courseName, int courseExpirationMonths) =>
        $"""
          <!DOCTYPE html>
          <html lang="en"><head>
              <meta charset="UTF-8">
              <meta name="viewport" content="width=device-width, initial-scale=1.0">
              <link rel="preconnect" href="https://fonts.googleapis.com">
              <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="">
              <link href="https://fonts.googleapis.com/css2?family=Sora:wght@100..800&amp;display=swap" rel="stylesheet">
              <title>Makassed LMS</title>
          </head>

          <body style="margin:0; padding:0; font-family: Sora, sans-serif; background-color: #f4f4f4;">
              <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="background-color: #f4f4f4; padding: 20px;">
                  <tbody><tr>
                      <td align="center">
                          <!-- Main Container -->
                      </td><td style="padding-top:0;padding-bottom:0;padding-right:10px;padding-left:10px">
                          <div class="m_-533078734955811462webkit" style="max-width:600px;margin-top:0;margin-bottom:0;margin-right:auto;margin-left:auto">
          
                              <table role="presentation" class="m_-533078734955811462outer" align="center" style="border-spacing:0;Margin:0 auto;width:100%;max-width:600px">
                                  <tbody>
          
                                      <tr>
                                          <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                      </tr>
          
                                      <tr>
                                          <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                      </tr>
          
                                      <tr>
                                          <td class="m_-533078734955811462one-column" align="center" style="padding-left:10px;padding-right:10px">
          
          
                                              <table role="presentation" align="center" border="0" cellpadding="0" cellspacing="0" width="100%" style="border-radius:4px;background-color:#efebe4" bgcolor="#efebe4">
                                                  <tbody>
          
                                                      <tr>
                                                          <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                      </tr>
          
                                                      <tr>
                                                          <td align="center" style="padding-top:0px;padding-left:20px;padding-right:20px">
                                                              <h2 style="color:#027e7b;font-size:28px;font-weight:bold;font-stretch:normal;font-style:normal;line-height:36px;letter-spacing:-0.5px">Course Expired!</h2>
                                                          </td>
                                                      </tr>
          
                                                      <tr>
                                                          <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                      </tr>
          
          
                                                      <tr>
                                                          <td width="0" height="22" style="font-size:1px;line-height:1px" bgcolor="#ffffff"></td>
                                                      </tr>
          
                                                      <tr>
                                                          <td align="left" style="padding-top:0px;padding-left:20px;padding-bottom:0px;padding-right:20px" bgcolor="#ffffff">
                                                              <p style="color:#05192d;font-size:16px;line-height:26px;font-weight:normal">The course {courseName} has expired, {courseExpirationMonths} has passed since you finished The course, consider retaking it.</p>
                                                          </td>
                                                      </tr>
          
                                                      <tr>
                                                          <td width="0" height="32" style="font-size:1px;line-height:1px" bgcolor="#ffffff"></td>
                                                      </tr>
          
                                                      
          
                                                      
          
          
          
                                                  </tbody>
                                              </table>
          
                                          </td>
                                      </tr>
          
          
                                  </tbody>
                              </table>
          
                          </div>
                      </td>
                      
                  </tr>
              </tbody></table>


          </body></html>
          """;

    public static string GetCoursePokeEmailBody(string courseName) =>
    $"""
      <!DOCTYPE html>
      <html lang="en"><head>
          <meta charset="UTF-8">
          <meta name="viewport" content="width=device-width, initial-scale=1.0">
          <link rel="preconnect" href="https://fonts.googleapis.com">
          <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin="">
          <link href="https://fonts.googleapis.com/css2?family=Sora:wght@100..800&amp;display=swap" rel="stylesheet">
          <title>Makassed LMS</title>
      </head>
      
      <body style="margin:0; padding:0; font-family: Sora, sans-serif; background-color: #f4f4f4;">
          <table role="presentation" width="100%" cellspacing="0" cellpadding="0" style="background-color: #f4f4f4; padding: 20px;">
              <tbody><tr>
                  <td align="center">
                      <!-- Main Container -->
                  </td><td style="padding-top:0;padding-bottom:0;padding-right:10px;padding-left:10px">
                      <div class="m_-533078734955811462webkit" style="max-width:600px;margin-top:0;margin-bottom:0;margin-right:auto;margin-left:auto">
      
                          <table role="presentation" class="m_-533078734955811462outer" align="center" style="border-spacing:0;Margin:0 auto;width:100%;max-width:600px">
                              <tbody>
      
                                  <tr>
                                      <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                  </tr>
      
                                  <tr>
                                      <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                  </tr>
      
                                  <tr>
                                      <td class="m_-533078734955811462one-column" align="center" style="padding-left:10px;padding-right:10px">
      
      
                                          <table role="presentation" align="center" border="0" cellpadding="0" cellspacing="0" width="100%" style="border-radius:4px;background-color:#efebe4" bgcolor="#efebe4">
                                              <tbody>
      
                                                  <tr>
                                                      <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                  </tr>
      
                                                  <tr>
                                                      <td align="center" style="padding-top:0px;padding-left:20px;padding-right:20px">
                                                          <h2 style="color:#027e7b;font-size:28px;font-weight:bold;font-stretch:normal;font-style:normal;line-height:36px;letter-spacing:-0.5px">Just a reminder from Admins</h2>
                                                      </td>
                                                  </tr>
      
                                                  <tr>
                                                      <td width="0" height="32" style="font-size:1px;line-height:1px"></td>
                                                  </tr>
      
      
                                                  <tr>
                                                      <td width="0" height="22" style="font-size:1px;line-height:1px" bgcolor="#ffffff"></td>
                                                  </tr>
      
                                                  <tr>
                                                      <td align="left" style="padding-top:0px;padding-left:20px;padding-bottom:0px;padding-right:20px" bgcolor="#ffffff">
                                                          <p style="color:#05192d;font-size:16px;line-height:26px;font-weight:normal">As for Admins, please check {courseName} course status as soon as possible.</p>
                                                      </td>
                                                  </tr>
      
                                                  <tr>
                                                      <td width="0" height="32" style="font-size:1px;line-height:1px" bgcolor="#ffffff"></td>
                                                  </tr>
      
                                                  
      
                                                  
      
      
      
                                              </tbody>
                                          </table>
      
                                      </td>
                                  </tr>
      
      
                              </tbody>
                          </table>
      
                      </div>
                  </td>
                  
              </tr>
          </tbody></table>
      
      
      </body></html>
      """;
}